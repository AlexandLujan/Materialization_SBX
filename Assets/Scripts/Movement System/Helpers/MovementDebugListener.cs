using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Movement
{
    public class MovementDebugListener : MonoBehaviour
    {
        [SerializeField] private PlayerMovementSystem movementSystem;

        private void OnEnable()
        {
            if (movementSystem == null) return;

            movementSystem.OnJumped += HandleJumped;
            movementSystem.OnLanded += HandleLanded;
            movementSystem.OnLeftGround += HandleLeftGround;
            movementSystem.OnMovementStarted += HandleMovementStarted;
            movementSystem.OnMovementStopped += HandleMovementStopped;
            movementSystem.OnSprintStarted += HandleSprintStarted;
            movementSystem.OnSprintStopped += HandleSprintStopped;
        }

        private void OnDisable()
        {
            if (movementSystem == null) return;

            movementSystem.OnJumped -= HandleJumped;
            movementSystem.OnLanded -= HandleLanded;
            movementSystem.OnLeftGround -= HandleLeftGround;
            movementSystem.OnMovementStarted -= HandleMovementStarted;
            movementSystem.OnMovementStopped -= HandleMovementStopped;
            movementSystem.OnSprintStarted -= HandleSprintStarted;
            movementSystem.OnSprintStopped -= HandleSprintStopped;
        }

        private void HandleJumped() { Debug.Log("[MovementFeedbackListener] Jump feedback"); }
        private void HandleLanded() { Debug.Log("[MovementFeedbackListener] Land feedback"); }
        private void HandleLeftGround() { Debug.Log("[MovementFeedbackListener] Left ground feedback"); }
        private void HandleMovementStarted() { Debug.Log("[MovementFeedbackListener] Movement started feedback"); }
        private void HandleMovementStopped() { Debug.Log("[MovementFeedbackListener] Movement stopped feedback"); }
        private void HandleSprintStarted() { Debug.Log("[MovementFeedbackListener] Sprint started feedback"); }
        private void HandleSprintStopped() { Debug.Log("[MovementFeedbackListener] Sprint stopped feedback"); }
    }
}

// Attach this script to the Player GameObject.
