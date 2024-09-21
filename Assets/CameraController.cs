using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

    public class CameraController : MonoBehaviour
    {
    public Transform Target;


    void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        transform.position = Target.position;

    }
}

