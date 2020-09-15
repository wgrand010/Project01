using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundDetection : MonoBehaviour
{
    public event Action GroundDetected = delegate { };
    public event Action GroundVanished = delegate { };

    private void OnTriggerEnter(Collider other)
    {
        GroundDetected?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        GroundVanished?.Invoke();
    }

}
