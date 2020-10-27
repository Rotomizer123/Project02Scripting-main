using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FPSInput))]
[RequireComponent(typeof(FPSMotor))]
public class PlayerController : MonoBehaviour
{
    FPSInput _input = null;
    FPSMotor _motor = null;

    [SerializeField] public static float _playerHealth = 3f;
    [SerializeField] float _moveSpeed = 0.1f;
    [SerializeField] float _turnSpeed = 6f;
    [SerializeField] float _jumpStrength = 10f;

    [SerializeField] AudioClip _damagePlayer = null;
    [SerializeField] AudioClip _playerDeath = null;

    private void Awake()
    {
        _input = GetComponent<FPSInput>();
        _motor = GetComponent<FPSMotor>();
    }

    private void OnEnable()
    {
        _input.MoveInput += OnMove;
        _input.RotateInput += OnRotate;
        _input.JumpInput += OnJump;
        _input.FireInput += OnFire;
    }

    private void OnDisable()
    {
        _input.MoveInput -= OnMove;
        _input.RotateInput -= OnRotate;
        _input.JumpInput -= OnJump;
        _input.FireInput -= OnFire;
    }

    void OnMove(Vector3 movement)
    {
        _motor.Move(movement * _moveSpeed);
    }

    void OnRotate(Vector3 rotation)
    {
        _motor.Turn(rotation.y * _turnSpeed);
        _motor.Look(rotation.x * _turnSpeed);
    }

    void OnJump()
    {
        _motor.Jump(_jumpStrength);
    }

    void OnFire()
    {
        _motor.Fire();
    }

    public void DamagePlayer()
    {
        _playerHealth -= 1f;
        HealthSlider.HealthBar(_playerHealth);

        if (_playerHealth <= 0)
        {
            GameObject audioObject = new GameObject("2DAudio - PlayerDeath");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = _playerDeath;
            audioSource.Play();
            Destroy(audioObject, _playerDeath.length);
            Level01Controller.LoseGame();
            Time.timeScale = 0;
        }
        else
        {
            GameObject audioObject = new GameObject("2DAudio - PlayerDamage");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = _damagePlayer;
            audioSource.Play();
            Destroy(audioObject, _damagePlayer.length);
        }
    }
}
