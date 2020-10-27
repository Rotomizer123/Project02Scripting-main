using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] AudioClip _coinCollect = null;

    Level01Controller _levelController;

    public float _coinSpin = 5f;

    void Start()
    {
        _levelController = GameObject.Find("LevelController").GetComponent<Level01Controller>();
    }

    void Update()
    {
        CoinRotate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FPSPlayer")
        {
            _levelController.IncreaseScore(10);
            GameObject audioObject = new GameObject("2DAudio - CoinCollect");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = _coinCollect;
            audioSource.Play();
            Destroy(audioObject, _coinCollect.length);
            Destroy(this.gameObject);
        }
    }

    private void CoinRotate()
    {
        gameObject.transform.Rotate(0f, _coinSpin, 0f, Space.Self);
    }
}
