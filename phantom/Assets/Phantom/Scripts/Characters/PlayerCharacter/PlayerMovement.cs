using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour, IPlayerControlled
{
    private const float Gravity = -9.81f;
    private const float TurningThreshold = 0.1f;

    [Header("Character movement")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float sprintSpeedMultiplier = 1.5f;
    [SerializeField] private float gravityMultiplier = 2.0f;
    [SerializeField] private float jumpingPower = 5.0f;

    [Header("Camera turning")] 
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Vector2 turningSpeed = new Vector2(1.0f, 1.0f);
    [SerializeField] private Vector2 verticalTurningClamp = new Vector2(-90f, 90f);
    
    private Vector3 _movementDirection;
    private Vector3 _turningDirection;
    private bool _sprint;

    protected void Start()
    {
        EventDispatcher.Instance.Raise(new AssumingCharacterControlEvent { ControlledCharacter = this });
    }

    protected void Update()
    {
        ProcessTurning();
        ProcessGravity();
        ProcessMoving();
    }

    private void ProcessMoving()
    {
        var speedMultiplier = _sprint ? movementSpeed * sprintSpeedMultiplier : movementSpeed;

        characterController.Move(transform.TransformDirection(_movementDirection) * (speedMultiplier * Time.deltaTime));
    }

    private void ProcessGravity()
    {
        if (characterController.isGrounded && _movementDirection.y < 0.0f)
        {
            _movementDirection.y = -1.0f;
            return;
        }
        
        _movementDirection.y += Gravity * gravityMultiplier * Time.deltaTime; 
    }

    private void ProcessTurning()
    {
        if (_turningDirection.sqrMagnitude < TurningThreshold)
        {
            return;
        }
        
        transform.Rotate(Vector3.up, _turningDirection.x * turningSpeed.x);

        // float cameraVerticalRotation = cameraRoot.localRotation.eulerAngles.x;
        // cameraVerticalRotation -= _turningDirection.y * turningSpeed.y * Time.deltaTime;
        // cameraVerticalRotation = Mathf.Clamp( cameraVerticalRotation, verticalTurningClamp.x, verticalTurningClamp.y);
        //
        // cameraRoot.localRotation = Quaternion.Euler(cameraVerticalRotation, 0.0f, 0.0f);
    }

    #region IMovableCharacter

    public void Move(Vector2 direction)
    {
        _movementDirection = new Vector3(direction.x, 0.0f, direction.y);
    }

    public void Jump()
    {
        if (!characterController.isGrounded)
        {
            return;
        }

        _movementDirection.y += jumpingPower;
    }

    public void Sprint(bool sprint)
    {
        _sprint = sprint;
    }

    public void LookAround(Vector2 direction)
    {
        _turningDirection = new Vector3(direction.x, direction.y,0.0f);
    }

    #endregion
}