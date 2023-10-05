using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    public GameObject target;

    public float verticalOffset; //dar un pequeño aire

    public void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = new Vector3(0, target.transform.position.y + verticalOffset, transform.position.z);
            
        }
    }
    
}
