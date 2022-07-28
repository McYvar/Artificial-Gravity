using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Variables
    [SerializeField] Vector3 _3DPosition;
    [SerializeField] Vector3 angle;
    [SerializeField] Transform cameraCenter;
    [SerializeField] Transform player;
    [SerializeField] float sensitivity = 1;

    #endregion


    void Start()
    {
        transform.position = _3DPosition;
        transform.rotation = Quaternion.Euler(angle);
    }


    void Update()
    {
        switch (PlayerController.perspectiveType)
        {
            case PerspectiveType._2D:

                break;

            case PerspectiveType._3D:
                cameraCenter.position = player.position;
                float xRot = Input.GetAxisRaw("Mouse Y");
                float yRot = Input.GetAxisRaw("Mouse X");
                cameraCenter.Rotate(Vector3.right * xRot * sensitivity);
                cameraCenter.Rotate(Vector3.up * yRot * sensitivity);
                break;
        }
    }
}
