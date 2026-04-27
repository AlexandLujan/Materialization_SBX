using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Materialization.Core.Input;

namespace Materialization.Features.Inventory.Input
{
    public class InventoryInputController : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private PlayerInputReader input;
        [SerializeField] private InventorySystem inventorySystem;

        private void Awake()
        {
            // Debug.Log("[InventoryInputController] Awake fired.");

            if (input == null)
                input = GetComponent<PlayerInputReader>();

            if (input == null)
                input = FindFirstObjectByType<PlayerInputReader>();

            if (inventorySystem == null)
                inventorySystem = GetComponent<InventorySystem>();

            if (inventorySystem == null)
                inventorySystem = GetComponentInParent<InventorySystem>();

            if (inventorySystem == null)
                inventorySystem = FindFirstObjectByType<InventorySystem>();

            // Debug.Log($"[InventoryInputController] Input found? {input != null}");
            // Debug.Log($"[InventoryInputController] InventorySystem found? {inventorySystem != null}");
        }

        private void Update()
        {
            // Debug.Log("[InventoryInputController] Update fired.");

            if (input == null || inventorySystem == null)
            {
                // Debug.LogWarning("[InventoryInputController] Missing input or inventorySystem.");
                return;
            }

            // Debug.Log($"[InventoryInputController] MenuPressed = {input.MenuPressed}, IsOpen = {inventorySystem.IsOpen}");

            HandleOpenClose();
            HandleNavigation();
            HandleSelection();
        }

        private void HandleOpenClose()
        {
            if (!inventorySystem.IsOpen)
            {
                if (input.MenuPressed)
                {
                    Debug.Log("[InventoryInputController] MenuPressed detected. Calling OpenInventory().");
                    Debug.Log($"[InventoryInputController] InventorySystem Instance ID = {inventorySystem.GetInstanceID()}");
                    inventorySystem.OpenInventory();
                }
                return;
            }

            if (input.InventoryBackPressed)
            {
                Debug.Log("[InventoryInputController] InventoryBackPressed detected. Calling CloseInventory().");
                Debug.Log($"[InventoryInputController] InventorySystem Instance ID = {inventorySystem.GetInstanceID()}");
                inventorySystem.CloseInventory();
            }
        }

        private void HandleNavigation()
        {
            if (!inventorySystem.IsOpen)
                return;

            if (input.InventoryLeftPressed)
                inventorySystem.MoveSelection(-1);

            if (input.InventoryRightPressed)
                inventorySystem.MoveSelection(1);
        }

        private void HandleSelection()
        {
            if (!inventorySystem.IsOpen)
                return;

            if (input.InventorySelectPressed)
                inventorySystem.SwapPalmWithFinger();
        }

        /*
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
        */
    }
}