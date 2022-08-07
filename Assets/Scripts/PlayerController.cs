using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PerspectiveType { _2D, _3D }

[SelectionBase]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Movement variables")]
    [SerializeField] float movementSpeed;
    [SerializeField, Range(0f, 0.1f)] float rotationSpeed;

    [Space(10), Header("Remapable buttons")]
    [SerializeField] KeyCode switchPespective;
    
    public static PerspectiveType perspectiveType;
    static bool canSwitchPerspective = false;

    Rigidbody rb;
    [SerializeField] Transform orientation;
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform cameraCenter;

    #endregion


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        perspectiveType = PerspectiveType._3D;
    }


    void FixedUpdate()
    {
        switch (perspectiveType)
        {
            case PerspectiveType._2D:

                break;


            case PerspectiveType._3D:
                Vector3 cameraForwardNoXRot = Quaternion.Euler(0, cameraCenter.localEulerAngles.y, cameraCenter.localEulerAngles.z) * Vector3.forward;
                Vector3 targetDirection = transform.position + transform.rotation * cameraForwardNoXRot;
                Debug.DrawLine(transform.position, targetDirection);
                Debug.DrawLine(playerCamera.position, transform.position);

                float dotX = Vector3.Dot(transform.right, targetDirection);
                float offsetAngleX = Mathf.Acos(dotX) * Mathf.Rad2Deg;

                float dotZ = Vector3.Dot(transform.forward, targetDirection);
                float offsetAngleZ = Mathf.Acos(dotZ) * Mathf.Rad2Deg;
                if (offsetAngleX > 90)
                {
                    offsetAngleZ *= -1;
                }
                Debug.Log(offsetAngleZ);

                float verticalInput = Input.GetAxis("Vertical");
                Debug.Log(verticalInput);
                if (verticalInput != 0)
                {
                    orientation.transform.localRotation = Quaternion.Slerp(orientation.transform.localRotation, Quaternion.Euler(0, offsetAngleZ, 0), rotationSpeed);
                }
                break;
        }

        if (Input.GetKeyDown(switchPespective)) SwitchPerspective();
    }


    public static void SwitchPerspective()
    {
        if (!canSwitchPerspective) return;
        switch (perspectiveType)
        {
            case PerspectiveType._2D:
                perspectiveType = PerspectiveType._3D;
                break;


            case PerspectiveType._3D:
                perspectiveType = PerspectiveType._2D;
                break;
        }
    }
}
