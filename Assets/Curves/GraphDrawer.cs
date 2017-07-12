using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public class GraphDrawer : MonoBehaviour {

    public enum CurvesMethods { Hermite, Sinerp, Coserp,Berp,
        QuadEaseIn, QuadEaseOut, QuadEaseInOut,
        CubicEaseIn, CubicEaseOut, CubicEaseInOut,
        QuartEaseIn, QuartEaseOut, QuartEaseInOut,
        QuintEaseIn, QuintEaseOut, QuintEaseInOut,
        SinEaseIn, SinEaseOut, SinEaseInOut,
        ExpEaseIn, ExpEaseOut, ExpEaseInOut,
        CircEaseIn, CircEaseOut, CircEaseInOut
    };
    public CurvesMethods curvesMethod;
    public int precision = 100;
    public float height = 10;
    public float length = 10;

	// Use this for initialization
	void Start () {
        CheckMethods(typeof(Curves));
    }
	
	// Update is called once per frame
	void OnDrawGizmosSelected() {
        Gizmos.color = Color.black;

        //Draw the box where the curve will be displayed
        Gizmos.DrawLine(Vector3.zero, Vector3.up * height);
        Gizmos.DrawLine(new Vector3(0, height), new Vector3(length, height));
        Gizmos.DrawLine(Vector3.zero, Vector3.right * length);
        Gizmos.DrawLine(new Vector3(length, 0), new Vector3(length, height));

        Gizmos.color = Color.red;
        MethodInfo method = FindMethod(typeof(Curves), curvesMethod.ToString());
        DrawPointArray(GetPointsFromFunction(method,precision));
    }

    void CheckMethods(Type type)
    {
        // Get the public static methods.
        MethodInfo[] myArrayMethodInfo = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
        Debug.Log("The number of public methods is "+ myArrayMethodInfo.Length);
        // Display all the methods.
        DisplayMethodInfo(myArrayMethodInfo);
        GetPointsFromFunction(myArrayMethodInfo[0], precision);
    }
    void DisplayMethodInfo(MethodInfo[] myArrayMethodInfo)
    {
        // Display information for all methods.
        for (int i = 0; i < myArrayMethodInfo.Length; i++)
        {
            MethodInfo myMethodInfo = (MethodInfo)myArrayMethodInfo[i];
            Debug.Log("The name of the method is "+ myMethodInfo.Name);
        }
    }

    //Find the method given by the curvesMethods variable and returns it
    MethodInfo FindMethod(Type type, String methodName)
    {
        //Find all the public static methods
        MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
        for(int i=0;i< methods.Length;i++)
        {
            //return the one we are looking for
            if (methods[i].Name == methodName)
                return methods[i];
        }

        return null;
    }

    //Calculate the position of a {precision} number of points using the method and store them into a vector3 array
    float[] GetPointsFromFunction(MethodInfo method, int precision)
    {
        float[] points = new float[precision];
        for (int i = 0; i < precision;i++)
        {
            //calculate the x coordinate of the point
            points[i] = (float)method.Invoke(null, new object[3] { 0f, 1f, i / (precision-1f) });
        }
        return points;
    }

    //Draw a curve from an array of points
    void DrawPointArray(float[] points)
    {
        Vector3 lastPoint = Vector3.zero;

        for(int i=0;i<points.Length;i++)
        {
            //Extract the x coordinate and calcul the y coordinate from the index i
            Vector3 currentPoint = new Vector3(i / (precision - 1f)*length, points[i]*height);
            Gizmos.DrawLine(lastPoint, currentPoint);
            lastPoint = currentPoint;
        }
    }
}