using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Movement
{
    public class PlayerFallState : PlayerMovementState
    {
        public PlayerFallState(PlayerMovementStateMachine stateMachine, PlayerMovementSystem movementSystem) : base(stateMachine, movementSystem) { }
        public override void Enter() { }
        public override void Tick()
        {
            if (movementSystem.IsGrounded)
            {
                if (movementSystem.IsMoving)
                    stateMachine.ChangeState(stateMachine.MoveState);
                else
                    stateMachine.ChangeState(stateMachine.IdleState);
            }
        }
        public override void Exit() { }
    }
}
