using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Movement
{
    public class PlayerMovementSystem : MonoBehaviour
    {
        [Header("Permission")]
        [SerializeField] private bool movementEnabled = true;
        [SerializeField] private bool jumpEnabled = true;
        [SerializeField] private bool sprintEnabled = true;

        public bool MovementEnabled => movementEnabled;
        public bool JumpEnabled => jumpEnabled;
        public bool SprintEnabled => sprintEnabled;

        public bool MovementLocked { get; private set; }

        public bool CanMove => movementEnabled && !MovementLocked;
        public bool CanJump => jumpEnabled && !MovementLocked;
        public bool CanSprint => sprintEnabled && !MovementLocked;

        public bool IsGrounded { get; private set; }
        public bool IsMoving { get; private set; }
        public bool IsSprinting { get; private set; }
        public bool IsJumping { get; private set; }
        public bool IsFalling { get; private set; }

        public float CurrentHorizontalSpeed { get; private set; }
        public float CurrentVerticalSpeed { get; private set; }

        public Vector3 GroundNormal { get; private set; } = Vector3.up;
        public float SlopeAngle { get; private set; }

        public event Action OnJumped;
        public event Action OnLanded;
        public event Action OnLeftGround;
        public event Action OnSprintStarted;
        public event Action OnSprintStopped;
        public event Action OnMovementStarted;
        public event Action OnMovementStopped;

        public void EnableMovement() => movementEnabled = true;
        public void DisableMovement() => movementEnabled = false;

        public void EnableJump() => jumpEnabled = true;
        public void DisableJump() => jumpEnabled = false;

        public void EnableSprint() => sprintEnabled = true;
        public void DisableSprint() => sprintEnabled = false;

        public void SetMovementLocked(bool locked) { MovementLocked = locked; }
        public void SetGrounded(bool grounded) 
        {
            if (IsGrounded != grounded)
                //Debug.Log($"[Movement System] SetGrounded: {grounded}");

            IsGrounded = grounded; 
        }

        public void SetGroundNormal(Vector3 normal) { GroundNormal = normal; }
        public void SetSlopeAngle(float angle) { SlopeAngle = angle; }
        public void SetMoving (bool moving)
        {
            if (IsMoving == moving) return;

            IsMoving = moving;

            //Debug.Log($"[Movement System] SetMoving: {moving}");

            if (moving)
                OnMovementStarted?.Invoke();
            else
                OnMovementStopped?.Invoke();
        }

        public void SetSprinting(bool sprinting)
        {
            if (IsSprinting == sprinting) return;

            IsSprinting = sprinting;

            //Debug.Log($"[Movement System] SetSprinting: {sprinting}");

            if (sprinting)
                OnSprintStarted?.Invoke();
            else
                OnSprintStopped?.Invoke();
        }

        public void SetHorizontalSpeed(float speed) { CurrentHorizontalSpeed = speed; }

        public void SetVerticalSpeed(float speed)
        {
            //Debug.Log($"[MovementSystem] SetVerticalSpeed | Grounded: {IsGrounded} | Jumping: {IsJumping} | Falling: {IsFalling} | Speed: {speed:F2}");
            CurrentVerticalSpeed = speed;

            if (!IsGrounded && speed < -0.01f)
            {
                IsFalling = true; 
                IsJumping = false;
            }
            else if (IsGrounded)
                IsFalling = false;
        }

        public void RaiseJumped()
        {
            IsJumping = true;
            IsFalling = false;
            IsGrounded = false;

            Debug.Log("[MovementSystem] RaiseJumped");

            OnJumped?.Invoke();
        }

        public void NotifyLanded()
        {
            IsJumping = false;
            IsFalling = false;
            IsGrounded = true;

            Debug.Log("[MovementSystem] NotifyLanded");

            OnLanded?.Invoke();
        }

        public void NotifyLeftGround()
        {
            IsGrounded = false;
            IsSprinting = false;

            if (!IsJumping)
                IsFalling = true;

            Debug.Log("[MovementSystem] NotifyLeftGround");

            OnLeftGround?.Invoke();
        }
    }
}
