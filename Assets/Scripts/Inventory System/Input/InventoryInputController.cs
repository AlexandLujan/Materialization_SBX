using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Materialization.Features.Inventory.Input
{
    public class InventoryInputController : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private InventorySystem inventorySystem;

        [Header("Action Map Names")]
        [SerializeField] private string playerMapName = "Player";
        [SerializeField] private string inventoryMapName = "Inventory";

        [Header("Action Names")]
        [SerializeField] private string openInventoryActionName = "Open Inventory";
        [SerializeField] private string moveLeftActionName = "Move Left";
        [SerializeField] private string moveRightActionName = "Move Right";
        [SerializeField] private string selectionActionName = "Selection";
        [SerializeField] private string backActionName = "Go Back";

        private InputAction openInventoryAction;
        private InputAction moveLeftAction;
        private InputAction moveRightAction;
        private InputAction selectionAction;
        private InputAction backAction;

        private void Awake()
        {
            if (playerInput == null) playerInput = GetComponent<PlayerInput>();

            CacheActions();
        }

        private void OnEnable()
        {
            SubscribeActions();
        }

        private void OnDisable()
        {
            UnsubscribeActions();
        }

        private void Start()
        {
            SwitchToPlayerMap();
        }

        private void CacheActions()
        {
            if (playerInput == null || playerInput.actions == null) return;

            InputActionMap playerMap = playerInput.actions.FindActionMap(playerMapName, true);
            InputActionMap inventoryMap = playerInput.actions.FindActionMap(inventoryMapName, true);

            openInventoryAction = playerMap.FindAction(openInventoryActionName, true);

            moveLeftAction = inventoryMap.FindAction(moveLeftActionName, true);
            moveRightAction = inventoryMap.FindAction(moveRightActionName, true);
            selectionAction = inventoryMap.FindAction(selectionActionName, true);
            backAction = inventoryMap.FindAction(backActionName, true);
        }

        private void SubscribeActions()
        {
            if (openInventoryAction != null) openInventoryAction.performed += OnOpenInventoryPerformed;
            if (moveLeftAction != null) moveLeftAction.performed += OnMoveLeftPerformed;
            if (moveRightAction != null) moveRightAction.performed += OnMoveRightPerformed;
            if (selectionAction != null) selectionAction.performed += OnSelectionPerformed;
            if (backAction != null) backAction.performed += OnBackPerformed;
        }

        private void UnsubscribeActions()
        {
            if (openInventoryAction != null) openInventoryAction.performed -= OnOpenInventoryPerformed;
            if (moveLeftAction != null) moveLeftAction.performed -= OnMoveLeftPerformed;
            if (moveRightAction != null) moveRightAction.performed -= OnMoveRightPerformed;
            if (selectionAction != null) selectionAction.performed -= OnSelectionPerformed;
            if (backAction != null) backAction.performed -= OnBackPerformed;
        }

        private void OnOpenInventoryPerformed(InputAction.CallbackContext context)
        {
            if (inventorySystem == null || inventorySystem.IsOpen) return;
            inventorySystem.OpenInventory();
            SwitchToInventoryMap();
        }

        private void OnMoveLeftPerformed(InputAction.CallbackContext context)
        {
            if (inventorySystem == null || !inventorySystem.IsOpen) return;
            inventorySystem.MoveSelection(-1);
        }

        private void OnMoveRightPerformed(InputAction.CallbackContext context)
        {
            if (inventorySystem == null || !inventorySystem.IsOpen) return;
            inventorySystem.MoveSelection(1);
        }

        private void OnSelectionPerformed(InputAction.CallbackContext context)
        {
            if (inventorySystem == null || !inventorySystem.IsOpen) return;
            inventorySystem.SwapPalmWithFinger();
        }

        private void OnBackPerformed(InputAction.CallbackContext context)
        {
            if (inventorySystem == null || !inventorySystem.IsOpen) return;
            inventorySystem.CloseInventory();
            SwitchToPlayerMap();
        }

        private void SwitchToPlayerMap()
        {
            if (playerInput != null) playerInput.SwitchCurrentActionMap(playerMapName);
        }

        private void SwitchToInventoryMap()
        {
            if (playerInput != null) playerInput.SwitchCurrentActionMap(inventoryMapName);
        }
    }
}