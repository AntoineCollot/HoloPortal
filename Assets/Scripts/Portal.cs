using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public static Portal Instance;
    static Camera cam;

	// Use this for initialization
	void Awake () {
        Instance = this;
        cam = Camera.main;
	}

    public static bool CamIsInFront()
    {
        return Vector3.Dot(Instance.transform.forward, cam.transform.position - Instance.transform.position) > 0;
    }
}
