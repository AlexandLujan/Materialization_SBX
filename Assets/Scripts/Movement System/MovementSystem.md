# Movement System Documentation

## 1. `PlayerInputReader.cs`
**Description**: This script reads player input from a Keyboard/Game Controller and stores values such as:
- Movement (WASD or Left Stick)
- Look (Mouse or Right Stick)
- Sprint (Shift or Left Trigger)
- Attack (Left Click or Right Trigger)
- Interact (X Key or Y/Triangle Button)
- Menu (F Key or X Button)

The job of this script its to act as a bridge between Unity Input actions and the rest of the Gameplay systems within.

## 2. `PlayerMovementSystem.cs`
**Description**: This script acts as the main movement controller of the system itself, it handles the following:
- Reading movement input from `PlayerInputReader.cs`.
- Converting input into world-space movement using the camera.
- Applying walking and sprinting speed.
- Handling acceleration/deceleration.
- Gravity/Vertical velocity.
- Ground checks.
- Exposing movement-related data to the state machine.

In summary, this script acts as the engine of the system.

## 3. `PlayerMovementStateMachine.cs`
**Description**: This script controls which movement state the player is currently in.
It switches between states such as:
- Idle
- Move
- Jump
- Fall

Its job is to organize movement behaviour cleanly instead of stuffing everything into one huge script.

## 4. `PlayerMovementState.cs`
**Description**: The base state class.

Usually contains a shared structure that follows this model:
```csharp
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
```

This gives all movement states a common format.

## 5. `GroundChecker.cs`
**Description**: This script checks whether the player is on the ground, uses a SphereCast to trigger actions for other scripts in the system to use.
This directly affects all of the states within the State Machine attached to the Movement System in a similar fashion.

## 6. `MovementDebugListener.cs`
**Description**: This script logs movement state changes in the Console, acts as an example for Listeners that may be present in other systems.

## 7. `Player[STATE_NAME]State.cs`
**Description**: There are scripts that represent each state in the State Machine, each having their own transition triggers.
