using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZoneDetector : MonoBehaviour
{
    Level01Controller levelController;
    [SerializeField] GameObject fpsPlayer = null;
    [SerializeField] AudioClip winZone = null;

    private void Awake()
    {
        levelController = GameObject.Find("LevelController").GetComponent<Level01Controller>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FPSPlayer")
        {
            GameObject audioObject = new GameObject("2DAudio - PlayerWin");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = winZone;
            audioSource.Play();
            Destroy(audioObject, winZone.length);
            levelController.WinGame();
        }
    }
}
