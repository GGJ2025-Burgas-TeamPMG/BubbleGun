using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target; 
    public float forcedZ = 10;
    public Vector3 offset; 

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        desiredPosition.z = forcedZ;
        transform.position = desiredPosition;
    }
}
