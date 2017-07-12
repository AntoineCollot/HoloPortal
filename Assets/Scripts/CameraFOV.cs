using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOV : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Camera>().fieldOfView = Camera.main.fieldOfView;
	}
}
