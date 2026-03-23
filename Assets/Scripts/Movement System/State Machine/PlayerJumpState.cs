using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Movement
{
    public class PlayerJumpState : PlayerMovementState
    {
        public PlayerJumpState(PlayerMovementStateMachine stateMachine, PlayerMovementSystem movementSystem) : base(stateMachine, movementSystem) { }
        public override void Enter() { }
        public override void Tick()
        {
            if (movementSystem.IsGrounded)
            {
                if (movementSystem.IsMoving)
                    stateMachine.ChangeState(stateMachine.MoveState);
                else
                    stateMachine.ChangeState(stateMachine.IdleState);
                return;
            }

            if (movementSystem.IsFalling) { stateMachine.ChangeState(stateMachine.FallState); }
        }
        public override void Exit() { }
    }
}
