using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    [SerializeField]
    float animTime;

    [SerializeField]
    float distanceFromCam;

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

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        StartCoroutine(SpawnC());
    }

    IEnumerator SpawnC()
    {
        yield return StartCoroutine(Scale(1, 0, animTime));

        //Move the transform at [distance] in front of the camera
        transform.position = cam.transform.position + cam.transform.forward * distanceFromCam;

        //Rotate it toward the camera
        Vector3 target = 2*transform.position - cam.transform.position;
        target.y = transform.position.y;
        transform.LookAt(target);

        StartCoroutine(Scale(0, 1, animTime));
    }

    IEnumerator Scale(float start,float end, float time)
    {
        float t = 0;

        while(t<1)
        {
            t += Time.deltaTime / time;

            float s = Curves.QuadEaseInOut(start, end, t);

            transform.localScale = s * Vector3.one;

            yield return null;
        }

        transform.localScale = end * Vector3.one;
    }
}
