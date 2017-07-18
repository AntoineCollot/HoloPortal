using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortalScaling : MonoBehaviour {

    [SerializeField]
    Transform portalParent;

    Camera cam;

	// Use this for initialization
	void Awake () {
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

        float distance = Vector3.Distance(portalParent.position, Camera.main.transform.position);

        transform.position = cam.transform.position + cam.transform.forward * distance;

        float h = Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad * 0.5f) * distance * 2f;

        transform.localScale = new Vector3(h * cam.aspect, h, 0f);
        transform.rotation = cam.transform.rotation;
    }

}
