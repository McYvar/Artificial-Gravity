using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Variables
    [SerializeField] float closeness;
    [SerializeField] Vector3 angle;
    [SerializeField] Transform cameraCenter;
    [SerializeField] Transform player;

    [SerializeField] float sensitivity = 1;
    private float xRotation; // for camera and for orientation
    private float yRotation; // for camera only

    #endregion


    void Start()
    {
        cameraCenter.rotation = Quaternion.Euler(angle);
        yRotation = cameraCenter.localEulerAngles.y;
        xRotation = cameraCenter.localEulerAngles.x;
    }


    void Update()
    {
        switch (PlayerController.perspectiveType)
        {
            case PerspectiveType._2D:

                break;

            case PerspectiveType._3D:
                transform.position = transform.forward * -closeness;
                cameraCenter.position = player.position;
                float xRot = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.fixedDeltaTime;
                float yRot = Input.GetAxisRaw("Mouse X") * sensitivity * Time.fixedDeltaTime;

                //yRotation += yRot;
                //xRotation -= xRot;
                //xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                //cameraCenter.localRotation = player.rotation;
                cameraCenter.Rotate(xRot, yRot, 0);
                break;
        }
    }
}
