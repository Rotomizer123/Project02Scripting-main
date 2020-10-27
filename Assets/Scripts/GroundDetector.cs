using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundDetector : MonoBehaviour
{
    public event Action GroundDetected = delegate { };
    public event Action GroundVanished = delegate { };

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(9))
        {
            Debug.Log("Grounded");
            GroundDetected?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer.Equals(9))
        {
            Debug.Log("Not Grounded");
            GroundVanished?.Invoke();
        }
    }
}
