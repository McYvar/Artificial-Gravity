using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PerspectiveType { _2D, _3D }

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Movement variables")]
    [SerializeField] float movementSpeed;

    [Space(10), Header("Remapable buttons")]
    [SerializeField] KeyCode switchPespective;
    
    public static PerspectiveType perspectiveType;
    static bool canSwitchPerspective = false;

    Rigidbody rb;
    [SerializeField] Transform orientation;
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
                //rb.AddForce(orientation.forward * movementSpeed * Input.GetAxis("Vertical"), ForceMode.VelocityChange);
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
