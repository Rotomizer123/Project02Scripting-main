using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class FPSMotor : MonoBehaviour
{
    public event Action Land = delegate { };

    [SerializeField] Camera _camera = null;
    [SerializeField] float _cameraAngleLimit = 70f;
    [SerializeField] GroundDetector _groundDetector = null;
    [SerializeField] AudioClip _fire = null;
    [SerializeField] AudioClip _enemyDeath = null;
    [SerializeField] ParticleSystem _gunBlast = null;

    Vector3 _movementThisFrame = Vector3.zero;
    float _turnAmountThisFrame = 0;
    float _lookAmountThisFrame = 0;
    private float _currentCameraRotationX = 0;
    bool _isGrounded = false;

    Rigidbody _rigidbody = null;
    ParticleSystem _muzzleFlash = null;
    RaycastHit _hitInfo;
    Level01Controller _levelController;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _muzzleFlash = this.transform.Find("Main Camera").Find("MuzzleFlash").gameObject.GetComponent<ParticleSystem>();
        _levelController = GameObject.Find("LevelController").GetComponent<Level01Controller>();
    }

    private void FixedUpdate()
    {
        ApplyMovement(_movementThisFrame);
        ApplyTurn(_turnAmountThisFrame);
        ApplyLook(_lookAmountThisFrame);
    }

    public void Move(Vector3 requestedMovement)
    {
        _movementThisFrame = requestedMovement;
    }

    public void Turn(float turnAmount)
    {
        _turnAmountThisFrame = turnAmount;
    }

    public void Look(float lookAmount)
    {
        _lookAmountThisFrame = lookAmount;
    }

    public void Jump(float jumpForce)
    {
        if (_isGrounded == false)
            return;
        _rigidbody.AddForce(Vector3.up * jumpForce);
    }

    public void Fire()
    {
        if (Time.timeScale != 0)
        {
            _muzzleFlash.Play();
            GameObject audioObject = new GameObject("2DAudio");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = _fire;
            audioSource.Play();
            Destroy(audioObject, _fire.length);
            if (Physics.Raycast(this.transform.Find("Main Camera").Find("Weapon").position, transform.Find("Main Camera").forward, out _hitInfo, 50f))
            {
                Vector3 endPoint = transform.Find("Main Camera").forward * 50f;
                Debug.DrawRay(this.transform.Find("Main Camera").Find("Weapon").position, endPoint, Color.green, 1);
                EnemyController enemyController = _hitInfo.transform.gameObject.GetComponent<EnemyController>();
                _gunBlast.transform.position = _hitInfo.point;
                Debug.Log(_hitInfo.normal);
                _gunBlast.Emit(50);
                Debug.Log("You hit the " + _hitInfo.transform.name);
                if (enemyController != null)
                {
                    _levelController.IncreaseScore(100);

                    GameObject audioObject1 = new GameObject("2DAudio - EnemyKill");
                    AudioSource audioSource1 = audioObject.AddComponent<AudioSource>();
                    audioSource1.clip = _enemyDeath;
                    audioSource1.Play();
                    Destroy(audioObject1, _enemyDeath.length);

                    enemyController.DestroyEnemy();
                }
            }
        }
    }

    void ApplyMovement(Vector3 moveVector)
    {
        if (moveVector == Vector3.zero)
            return;
        _rigidbody.MovePosition(_rigidbody.position + moveVector);
        _movementThisFrame = Vector3.zero;
    }

    void ApplyTurn(float rotateAmount)
    {
        if (rotateAmount == 0)
            return;
        Quaternion newRotation = Quaternion.Euler(0, rotateAmount, 0);
        _rigidbody.MoveRotation(_rigidbody.rotation * newRotation);
        _turnAmountThisFrame = 0;
    }

    void ApplyLook(float lookAmount)
    {
        if (lookAmount == 0)
            return;

        _currentCameraRotationX -= lookAmount;
        _currentCameraRotationX = Mathf.Clamp(_currentCameraRotationX, -_cameraAngleLimit, _cameraAngleLimit);
        _camera.transform.localEulerAngles = new Vector3(_currentCameraRotationX, 0, 0);
        _lookAmountThisFrame = 0;
    }

    private void OnEnable()
    {
        _groundDetector.GroundDetected += OnGroundDetected;
        _groundDetector.GroundVanished += OnGroundVanished;
    }

    private void OnDisable()
    {
        _groundDetector.GroundDetected -= OnGroundDetected;
        _groundDetector.GroundVanished -= OnGroundVanished;
    }

    void OnGroundDetected()
    {
        _isGrounded = true;
        Land?.Invoke();
    }

    void OnGroundVanished()
    {
        _isGrounded = false;
    }
}
