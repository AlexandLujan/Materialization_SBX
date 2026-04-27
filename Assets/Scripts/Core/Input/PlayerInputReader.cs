using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Core.Input
{
    public class PlayerInputReader : MonoBehaviour
    {
        private PlayerControls controls;
        public InputMode CurrentMode { get; private set; } = InputMode.Player;

        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public float CameraZoom { get; private set; }

        public bool SprintHeld { get; private set; }
        public bool CameraModeActive { get; private set; }

        public bool JumpPressed { get; private set; }
        public bool AttackPressed { get; private set; }
        public bool MenuPressed { get; private set; }

        public bool InteractPressed { get; private set; }

        public bool InventoryLeftPressed { get; private set; }
        public bool InventoryRightPressed { get; private set; }
        public bool InventorySelectPressed { get; private set; }
        public bool InventoryBackPressed { get; private set; }

        private void Awake()
        {
            controls = new PlayerControls();

            controls.Player.Move.performed += ctx => Move = ctx.ReadValue<Vector2>();
            controls.Player.Move.canceled += _ => Move = Vector2.zero;

            controls.Player.Look.performed += ctx => Look = ctx.ReadValue<Vector2>();
            controls.Player.Look.canceled += _ => Look = Vector2.zero;

            controls.Player.CameraZoom.performed += ctx =>
            {
                CameraZoom = ctx.ReadValue<float>();
                Debug.Log("Zoom fired: " + CameraZoom);
            };
            controls.Player.CameraZoom.canceled += _ => CameraZoom = 0f;

            controls.Player.Sprint.performed += _ => SprintHeld = true;
            controls.Player.Sprint.canceled += _ => SprintHeld = false;

            controls.Player.Jump.performed += _ => JumpPressed = true;
            controls.Player.AttackUseMaterial.performed += _ => AttackPressed = true;
            controls.Player.Menu.performed += _ => MenuPressed = true;
            controls.Player.Interact.performed += _ => InteractPressed = true;

            controls.Player.ToggleCamera.performed += _ =>
            {
                CameraModeActive = !CameraModeActive;
                UpdateCursorState();
            };

            controls.Inventory.MoveLeft.performed += _ => InventoryLeftPressed = true;
            controls.Inventory.MoveRight.performed += _ => InventoryRightPressed = true;
            controls.Inventory.Selection.performed += _ => InventorySelectPressed = true;
            controls.Inventory.GoBack.performed += _ => InventoryBackPressed = true;
        }
        private void OnEnable()
        {
            SetInputMode(InputMode.Player);
        }

        private void OnDisable()
        {
            controls.Player.Disable();
            controls.Inventory.Disable();
        }

        private void LateUpdate()
        {
            JumpPressed = false;
            AttackPressed = false;
            MenuPressed = false;
            InteractPressed = false;

            InventoryLeftPressed = false;
            InventoryRightPressed = false;
            InventorySelectPressed = false;
            InventoryBackPressed = false;
        }

        public void SetInputMode(InputMode mode)
        {
            Debug.Log($"[PlayerInputReader] SetInputMode called -> {mode}");

            CurrentMode = mode;

            controls.Player.Disable();
            controls.Inventory.Disable();

            Move = Vector2.zero;
            Look = Vector2.zero;
            CameraZoom = 0f;
            SprintHeld = false;

            switch (mode)
            {
                case InputMode.Player:
                    controls.Player.Enable();
                    Debug.Log($"[PlayerInputReader] Player enabled: {controls.Player.enabled}, Inventory enabled: {controls.Inventory.enabled}");
                    break;

                case InputMode.Inventory:
                    controls.Inventory.Enable();
                    Debug.Log($"[PlayerInputReader] Player enabled: {controls.Player.enabled}, Inventory enabled: {controls.Inventory.enabled}");
                    break;
            }
        }

        public float ConsumeZoom()
        {
            float zoom = CameraZoom;
            CameraZoom = 0f;
            return zoom;
        }

        private void UpdateCursorState()
        {
            if (CameraModeActive)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
