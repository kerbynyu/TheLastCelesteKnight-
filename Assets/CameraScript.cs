using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.0125f;
    public Vector3 offset; 


    private void FixedUpdate() {
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition,smoothSpeed*Time.deltaTime);
       
        transform.position = smoothedPosition + offset;
    }
}
