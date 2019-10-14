using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float damping;


    Vector3 trackedPosition;

    Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(target)
        {
            trackedPosition = target.position;
            trackedPosition.z = -10;
            transform.position = Vector3.SmoothDamp(transform.position, trackedPosition, ref velocity, damping);
        }
    }
}
