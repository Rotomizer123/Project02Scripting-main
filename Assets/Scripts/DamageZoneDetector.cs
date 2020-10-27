using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZoneDetector : MonoBehaviour
{
    PlayerController fpsScript;
    [SerializeField] GameObject fpsPlayer = null;

    private void Awake()
    {
        fpsScript = fpsPlayer.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FPSPlayer")
        {
            fpsScript.DamagePlayer();
        }
    }
}
