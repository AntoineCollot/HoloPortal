using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCollision : MonoBehaviour {

    [Header("Views")]

    [SerializeField]
    GameObject holoView;

    [SerializeField]
    GameObject blackView;

    bool entranceCamIsInFront;

    private void OnTriggerEnter(Collider other)
    {
        CameraMode.Instance.ChangeMode();

        entranceCamIsInFront = Portal.CamIsInFront();
    }

    private void OnTriggerExit(Collider other)
    {
        //If the user exit from the same side he entered, revert the mode change
        if(Portal.CamIsInFront() == entranceCamIsInFront)
        {
            CameraMode.Instance.ChangeMode();
        }
        //Else apply the changes to the views
        else
        {
            holoView.SetActive(!holoView.activeInHierarchy);
            blackView.SetActive(!blackView.activeInHierarchy);
        }
    }
}
