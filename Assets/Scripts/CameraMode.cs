using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMode : MonoBehaviour {

    [SerializeField]
    LayerMask realMask;

    [SerializeField]
    LayerMask holoMask;

    Camera cam;

    public static CameraMode Instance;

    public enum Mode { Real,Holo}
    Mode mode;

	// Use this for initialization
	void Awake () {
        Instance = this;
        cam = GetComponent<Camera>();
	}
	
	public void SetReal()
    {
        mode = Mode.Real;
        cam.cullingMask = realMask;
    }

    public void SetHolo()
    {
        mode = Mode.Holo;
        cam.cullingMask = holoMask;
    }

    public void ChangeMode()
    {
        switch(mode)
        {
            case Mode.Real:
                SetHolo();
                break;
            case Mode.Holo:
                SetReal();
                break;
        }
    }
}
