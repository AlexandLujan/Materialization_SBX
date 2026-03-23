using Materialization.Core.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Movement
{
    public class PlayerMovementStateMachine : MonoBehaviour
    {
        //[Header("References")]
        //[SerializeField] private PlayerInputReader input;
        //[SerializeField] private GroundChecker groundChecker;
        //[SerializeField] private PlayerController3D controller;

        [SerializeField] private PlayerMovementSystem movementSystem;
        public PlayerMovementState CurrentState { get; private set; }

        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerFallState FallState { get; private set; }

        private void Awake()
        {
            //if (input == null)
            //    input = GetComponent<PlayerInputReader>();
            //if (groundChecker == null)
            //    groundChecker = GetComponent<GroundChecker>();
            //if (controller == null)
            //    controller = GetComponent<PlayerController3D>();

            if (movementSystem == null)
                movementSystem = GetComponent<PlayerMovementSystem>();

            IdleState = new PlayerIdleState(this, movementSystem);
            MoveState = new PlayerMoveState(this, movementSystem);
            JumpState = new PlayerJumpState(this, movementSystem);
            FallState = new PlayerFallState(this, movementSystem);
        }

        private void Start()
        {
            ChangeState(IdleState);
        }

        private void Update()
        {
            if (movementSystem == null || CurrentState == null) return;

            // Debug.Log($"[StateMachine] CurrentState: {CurrentState.GetType().Name}");
            CurrentState.Tick();
        }

        public void ChangeState(PlayerMovementState newState)
        {
            if (newState == null || CurrentState == newState) return;

            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        /// <summary>
        /// LEGACY: EvaluateState()
        /// </summary>
        /*
        private void EvaluateState()
        {
            if (input == null || groundChecker == null || controller == null)
                return;

            PlayerMovementState nextState = DetermineState();
        }

        private PlayerMovementState DetermineState()
        {
            bool isGrounded = groundChecker.IsGrounded;
            bool justLanded = groundChecker.JustLanded;
            bool hasMoveInput = input.Move.sqrMagnitude > 0.01f;
            bool sprintHeld = input.SprintHeld;

            float verticalSpeed = controller.VerticalVelocity.y;

            // Temporary State Output
            if (justLanded)
                return PlayerMovementState.Land;

            if (!isGrounded)
            {
                if (verticalSpeed > 0.1f)
                    return PlayerMovementState.Jump;

                return PlayerMovementState.Fall;

            }

            if (hasMoveInput)
            {
                if (sprintHeld)
                    return PlayerMovementState.Sprint;

                return PlayerMovementState.Walk;
            }

            return PlayerMovementState.Idle;
        }
        */
    }
}

// Developer Notes
// If I need any specific functionality involving the speed, I'll need to expose currentSpeed as public without owning it.