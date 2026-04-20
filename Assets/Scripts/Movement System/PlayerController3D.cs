using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Materialization.Core.Input;
using Materialization.Features.Inventory;

namespace Materialization.Features.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController3D : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private PlayerInputReader input;
        [SerializeField] private GroundChecker groundChecker;
        [SerializeField] private PlayerMovementSystem movementSystem;
        // Debug fix for now, I will remove this when I implement the Input Handling System.
        [SerializeField] private InventorySystem inventorySystem;

        [Header("Movement")]
        [SerializeField] private float walkSpeed = 6f;
        [SerializeField] private float sprintSpeed = 9f;
        [SerializeField] private float accelerationTime = 0.5f;
        [SerializeField] private float decelerationTime = 0.2f;
        [SerializeField] private float rotationSpeed = 12.0f;

        [Header("Jump/Gravity")]
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float gravity = -25f;

        private CharacterController controller;
        private float currentSpeed;
        private Vector3 verticalVelocity;

        public Vector3 VerticalVelocity => verticalVelocity;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            HandleMovement();
            // I might need to expand upon this using more helper methods such as HandleHorizontalMovement() or HandleRotation().
            HandleActions();
        }

        private void HandleMovement()
        {
            bool grounded = groundChecker.IsGrounded;

            if (groundChecker.JustLanded)
                Debug.Log("[PlayerController3D] GroundChecker says: JustLanded");

            if (groundChecker.JustLeftGround)
                Debug.Log("[PlayerController3D] GroundChecker says: JustLeftGround");

            movementSystem.SetGrounded(grounded);
            movementSystem.SetGroundNormal(groundChecker.GroundNormal);
            movementSystem.SetSlopeAngle(groundChecker.SlopeAngle);

            if (groundChecker.JustLanded)
                movementSystem.NotifyLanded();

            if (groundChecker.JustLeftGround)
                movementSystem.NotifyLeftGround();

            if (!movementSystem.CanMove)
            {
                Debug.Log("[PlayerController3D] Movement blocked by MovementSystem.CanMove = false");

                movementSystem.SetMoving(false);
                movementSystem.SetSprinting(false);

                if (grounded && verticalVelocity.y > 0)
                    verticalVelocity.y = -2f;

                verticalVelocity.y += gravity * Time.deltaTime;
                controller.Move(verticalVelocity * Time.deltaTime);

                movementSystem.SetVerticalSpeed(verticalVelocity.y);
                movementSystem.SetHorizontalSpeed(0f);
            }

            Vector2 moveInput = input.Move;
            bool sprinting = input.SprintHeld;

            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = (camForward * moveInput.y + camRight * moveInput.x).normalized;

            float maxSpeed = sprinting ? sprintSpeed : walkSpeed;
            float targetSpeed = moveInput.magnitude > 0.01f ? maxSpeed : 0f;

            float accelRate = maxSpeed / Mathf.Max(accelerationTime, 0.01f);
            float decelRate = maxSpeed / Mathf.Max(decelerationTime, 0.01f);

            if (targetSpeed > currentSpeed)
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accelRate * Time.deltaTime);
            else
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, decelRate * Time.deltaTime);

            Vector3 horizontalMove = moveDirection * currentSpeed;

            if (moveDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            if (grounded && verticalVelocity.y < 0f)
                verticalVelocity.y = -2f;

            if (input.JumpPressed && grounded && controller.isGrounded)
            {
                Debug.Log("[PlayerController3D] Jump input accepted, calling MovementSystem.RaiseJumped()");
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                movementSystem.RaiseJumped();
            }

            verticalVelocity.y += gravity * Time.deltaTime;

            Vector3 finalMove = horizontalMove + verticalVelocity;
            controller.Move(finalMove * Time.deltaTime);

            movementSystem.SetGrounded(groundChecker.IsGrounded);
            movementSystem.SetGroundNormal(groundChecker.GroundNormal);
            movementSystem.SetSlopeAngle(groundChecker.SlopeAngle);
            movementSystem.SetMoving(moveInput.magnitude > 0.01f);
            movementSystem.SetSprinting(sprinting);
            movementSystem.SetHorizontalSpeed(new Vector3(horizontalMove.x, 0f, horizontalMove.z).magnitude);
            movementSystem.SetVerticalSpeed(verticalVelocity.y);
        }

        private void HandleActions()
        {
            if (inventorySystem != null && inventorySystem.IsOpen)
                return;

            // Will likely require some sort of Animation and Feedback communication system.
            if (input.AttackPressed)
                Debug.Log("Attack Triggered");

            // Will likely require some sort of Input Action communication system.
            if (input.MenuPressed)
                Debug.Log("Menu Opened");

            // Will likely require some sort of Input Action communication system.
            if (input.InteractPressed)
                Debug.Log("Interact Pressed");
        }
    }
}
