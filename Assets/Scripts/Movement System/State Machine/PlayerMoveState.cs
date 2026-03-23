using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Movement
{
    public class PlayerMoveState : PlayerMovementState
    {
        public PlayerMoveState(PlayerMovementStateMachine stateMachine, PlayerMovementSystem movementSystem) : base(stateMachine, movementSystem) { }

        public override void Enter() { }
        public override void Tick()
        {
            if (!movementSystem.IsGrounded)
            {
                if (movementSystem.IsFalling)
                {
                    stateMachine.ChangeState(stateMachine.FallState);
                    return;
                }

                if (movementSystem.IsJumping)
                {
                    stateMachine.ChangeState(stateMachine.JumpState);
                    return;
                }
            }

            if (!movementSystem.IsMoving) { stateMachine.ChangeState(stateMachine.IdleState); }
        }
        public override void Exit() { }
    }
}
