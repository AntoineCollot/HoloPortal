using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCollision : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        CameraMode.Instance.ChangeMode();
    }

    private void OnTriggerExit(Collider other)
    {
        if(Portal.CamIsInFront())
        {
            CameraMode.Instance.SetHolo();
        }
        else
        {
            CameraMode.Instance.SetReal();
        }
    }
}
