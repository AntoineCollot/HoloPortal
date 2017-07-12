using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour {

    [SerializeField]
    int field;

    [SerializeField]
    float axisSpeed;

    [SerializeField]
    float rotationSpeed;

    Quaternion axis;
    int dir = 1;
    float value = 0;
	
	// Update is called once per frame
	void Update () {
        value += axisSpeed * Time.deltaTime * dir;

        if (field == 0)
            axis.x = value;
        else if (field == 1)
            axis.y = value;

        if (value > 100000)
        {
            dir = -1;
            value = 100000;
        }
        else if(value < 0)
        {
            dir = 1;
            value = 0;
        }

        transform.Rotate(axis.eulerAngles, rotationSpeed * Time.deltaTime);
	}
}
