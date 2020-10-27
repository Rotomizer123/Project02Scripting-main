using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject _fpsPlayer = null;
    [SerializeField] GameObject _enemyBullet = null;
    [SerializeField] GameObject _deathParticles = null;
    [SerializeField] AudioClip _spottedSound = null;
    [SerializeField] AudioClip _bulletFire = null;

    RaycastHit rayHit;

    public double fireRate = 2;

    bool playerSpotted = false;

    private void Update()
    {
        CheckRange();
    }

    public void LookAtPlayer()
    {
        Transform target = _fpsPlayer.GetComponent<Transform>();
        Vector3 targetPosition = new Vector3(target.position.x, this.transform.position.y, target.position.z);
        this.transform.LookAt(targetPosition);
    }

    public void FireAtPlayer()
    { 
        Vector3 enemyPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1.2f, this.gameObject.transform.position.z);
        GameObject bullet = Instantiate(_enemyBullet, enemyPosition, Quaternion.identity);
        BulletControl bulletController = bullet.GetComponent<BulletControl>();
        bulletController._bulletDeath = _deathParticles.GetComponent<ParticleSystem>();
        bulletController.MoveBullet(_fpsPlayer.transform.position, this.transform.rotation);

        GameObject audioObject = new GameObject("2DAudio - bulletFire");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = _bulletFire;
        audioSource.Play();
        Destroy(audioObject, _bulletFire.length);

        StartCoroutine(bulletController.bulletLifetime(5));
    }

    public void CheckRange()
    {
        Vector3 enemyPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1.2f, this.gameObject.transform.position.z);
        Vector3 fireDirection = _fpsPlayer.transform.position - enemyPosition;
        if (Physics.Raycast(enemyPosition, fireDirection, out rayHit, 20f) && rayHit.collider == _fpsPlayer.GetComponent<CapsuleCollider>())
        {
            LookAtPlayer();
            if (rayHit.distance <= 20f && rayHit.collider == _fpsPlayer.GetComponent<CapsuleCollider>() && playerSpotted == false)
            {
                Debug.DrawRay(enemyPosition, fireDirection, Color.blue, 1);
                //Debug.Log("Enemy Spotted!");

                GameObject audioObject = new GameObject("2DAudio - PlayerSpotted");
                AudioSource audioSource = audioObject.AddComponent<AudioSource>();
                audioSource.clip = _spottedSound;
                audioSource.Play();
                Destroy(audioObject, _spottedSound.length);

                FireAtPlayer();
                playerSpotted = true;
            }
            if (playerSpotted == true && fireRate > 0)
            {
                fireRate -= Time.deltaTime;
            }
            else
            {
                fireRate = 2;
                playerSpotted = false;
            }
        }
    }

    public void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }
}
