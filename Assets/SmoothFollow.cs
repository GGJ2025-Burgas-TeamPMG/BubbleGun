using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target; // The object to follow
    public float smoothSpeed = 0.125f; // The smoothing speed
    public float forcedZ = 10;
    public Vector3 offset; // Offset relative to the target

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned to SmoothFollow script.");
            return;
        }

        // Calculate the desired position
        Vector3 desiredPosition = target.position + offset;
        desiredPosition.z = forcedZ;

        // Smoothly interpolate between the current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;
    }
}
