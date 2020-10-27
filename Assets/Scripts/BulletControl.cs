using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletControl : MonoBehaviour
{
    [SerializeField] Rigidbody _rb = null;
    [SerializeField] GameObject _target = null;
    [SerializeField] GameObject _enemy = null;
    [SerializeField] AudioClip _bulletExplode = null;

    public ParticleSystem _bulletDeath = null;

    PlayerController fpsScript = null;

    public float bulletSpeed = 5f;
    public float bulletRotation = 5f;

    private void Awake()
    {
        fpsScript = _target.GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        RotateBullet();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this != null)
        {
            if (!(other.gameObject.name.Contains("EnemyUnit")))
            {
                _bulletDeath.transform.position = this.transform.position;
                _bulletDeath.Emit(100);
                GameObject audioObject = new GameObject("2DAudio - BulletHit");
                AudioSource audioSource = audioObject.AddComponent<AudioSource>();
                audioSource.clip = _bulletExplode;
                audioSource.Play();
                Destroy(audioObject, _bulletExplode.length);
                Destroy(this.gameObject);
                if (other.gameObject.name.Contains("FPSPlayer"))
                {
                    fpsScript.DamagePlayer();
                }
            }
        }
    }

    public void MoveBullet(Vector3 target, Quaternion rotation)
    {
        if (_rb != null )
        {
            //Debug.Log(target);
            _rb.velocity = ((target - gameObject.transform.position).normalized * bulletSpeed);
            _rb.rotation = rotation;
        }
        else
        {
            Debug.Log("No Rigidbody");
        }
    }

    public void RotateBullet()
    {
        gameObject.transform.Rotate(0f, 0f, bulletRotation, Space.Self);
    }

    public IEnumerator bulletLifetime(int timer)
    {
        yield return new WaitForSeconds(timer);
        if (this != null)
            Destroy(this.gameObject);
    }
}
