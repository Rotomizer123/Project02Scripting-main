using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FPSInput : MonoBehaviour
{
    [SerializeField] bool _invertVertical = false;
    [SerializeField] float _sprintMultiplier = 3f;

    public event Action<Vector3> MoveInput = delegate { };
    public event Action<Vector3> RotateInput = delegate { };
    public event Action JumpInput = delegate { };
    public event Action FireInput = delegate { };

    // Update is called once per frame
    void Update()
    {
        DetectMoveInput();
        DetectRotateInput();
        DetectJumpInput();
        DetectFireInput();
    }

    private void DetectJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpInput?.Invoke();
        }
    }

    private void DetectRotateInput()
    {
        float xInput = Input.GetAxisRaw("Mouse X");
        float yInput = Input.GetAxisRaw("Mouse Y");

        if (xInput != 0 || yInput != 0)
        {
            if (_invertVertical == true)
            {
                yInput = -yInput;
            }
            Vector3 rotation = new Vector3(yInput, xInput, 0);
            RotateInput?.Invoke(rotation);
        }
    }

    private void DetectMoveInput()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        if (xInput != 0 || yInput != 0)
        {
            Vector3 _horizontalMovement = transform.right * xInput;
            Vector3 _forwardMovement = transform.forward * yInput;
            Vector3 movement = (_horizontalMovement + _forwardMovement).normalized;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movement *= _sprintMultiplier;
            }
            MoveInput?.Invoke(movement);
        }
    }

    private void DetectFireInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireInput?.Invoke();
        }
    }
}
