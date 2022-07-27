using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Variables


    #endregion


    void Update()
    {
        switch (PlayerController.perspectiveType)
        {
            case PerspectiveType._2D:

                break;

            case PerspectiveType._3D:

                break;
        }
    }
}
