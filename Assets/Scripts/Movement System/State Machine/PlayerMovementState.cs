using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Movement
{
    public abstract class PlayerMovementState
    {
        protected readonly PlayerMovementStateMachine stateMachine;
        protected readonly PlayerMovementSystem movementSystem;

        protected PlayerMovementState(PlayerMovementStateMachine stateMachine, PlayerMovementSystem movementSystem)
        {
            this.stateMachine = stateMachine;
            this.movementSystem = movementSystem;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Tick() { }
    }

    /*
    public enum PlayerMovementState
    {
        Idle,
        Walking,
        Sprinting,
        Jumping,
        Falling,
        Landed
    }
    */ 
}
