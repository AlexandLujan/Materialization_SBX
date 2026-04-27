using JetBrains.Annotations;
using Materialization.Core.Input;
using Materialization.Features.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Inventory
{
    public class InventorySystem : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader inputReader;
        [SerializeField] private InventorySlot equippedSlot = new InventorySlot();
        [SerializeField] private InventoryCategory materialCategory;
        [SerializeField] private int selectedFingerIndex = 0;
        [SerializeField] private bool isOpen;

        public bool IsOpen => isOpen;
        public InventorySlot EquippedSlot => equippedSlot;
        public List<InventorySlot> FingerSlots => materialCategory != null ? materialCategory.slots : null;
        public int SelectedFingerIndex => selectedFingerIndex;

        public event Action OnInventoryChanged;
        public event Action<int> OnSelectionChanged;
        public event Action<bool> OnInventoryOpenStateChanged;

        private void Awake()
        {
            if (inputReader == null)
                inputReader = FindFirstObjectByType<PlayerInputReader>();
        }

        private void Start()
        {
            if (FingerSlots != null && FingerSlots.Count > 0)
            {
                if (selectedFingerIndex < 0 || selectedFingerIndex >= FingerSlots.Count)
                    selectedFingerIndex = 0;
            }
            else
            {
                selectedFingerIndex = 0;
            }

            OnInventoryChanged?.Invoke();
            OnSelectionChanged?.Invoke(selectedFingerIndex);
            OnInventoryOpenStateChanged?.Invoke(isOpen);
        }

        public void SelectFinger(int index)
        {
            if (FingerSlots == null || FingerSlots.Count == 0)
                return;

            if (index < 0 || index >= FingerSlots.Count)
                return;

            if (selectedFingerIndex == index)
                return;

            selectedFingerIndex = index;
            OnSelectionChanged?.Invoke(selectedFingerIndex);
            OnInventoryChanged?.Invoke();
        }

        public void SelectNextFinger()
        {
            MoveSelection(1);
        }

        public void SelectPreviousFinger()
        {
            MoveSelection(-1);
        }

        public void SwapPalmWithFinger()
        {
            if (FingerSlots == null || FingerSlots.Count == 0)
                return;

            if (selectedFingerIndex < 0 || selectedFingerIndex >= FingerSlots.Count)
                return;

            InventorySlot selectedSlot = FingerSlots[selectedFingerIndex];
            if (selectedSlot == null)
                return;

            equippedSlot.SwapWith(selectedSlot);
            OnInventoryChanged?.Invoke();
            OnSelectionChanged?.Invoke(SelectedFingerIndex);
        }

        public void OpenInventory()
        {
            Debug.Log($"[InventorySystem] OpenInventory() called. Instance ID = {GetInstanceID()}");

            if (isOpen) return;

            isOpen = true;

            if (inputReader != null)
            {
                Debug.Log($"[InventorySystem] Calling SetInputMode(Inventory) on PlayerInputReader instance {inputReader.GetInstanceID()}");
                inputReader.SetInputMode(InputMode.Inventory);
            }
            else
            {
                Debug.LogWarning("[InventorySystem] inputReader is null.");
            }

            OnInventoryChanged?.Invoke();
            OnSelectionChanged?.Invoke(selectedFingerIndex);
            OnInventoryOpenStateChanged?.Invoke(true);
        }

        public void CloseInventory()
        {
            if (!isOpen) return;

            isOpen = false;

            if (inputReader != null)
                inputReader.SetInputMode(InputMode.Player);

            OnInventoryChanged?.Invoke();
            OnInventoryOpenStateChanged?.Invoke(false);
        }

        public void ToggleInventory()
        {
            if (isOpen) CloseInventory();
            else OpenInventory();
        }

        public InventorySlot GetSelectedSlot()
        {
            if (FingerSlots == null || FingerSlots.Count == 0)
                return null;

            if (selectedFingerIndex < 0 || selectedFingerIndex >= FingerSlots.Count)
                return null;

            return FingerSlots[selectedFingerIndex];
        }

        public void MoveSelection(int direction)
        {
            if (!isOpen) return;
            if (FingerSlots == null || FingerSlots.Count == 0) return;
            if (direction == 0) return;

            int previousIndex = selectedFingerIndex;
            int nextIndex = selectedFingerIndex;
            int attempts = 0;

            do
            {
                nextIndex += direction;

                if (nextIndex < 0)
                    nextIndex = FingerSlots.Count - 1;
                else if (nextIndex >= FingerSlots.Count)
                    nextIndex = 0;
                attempts++;
            }
            while (attempts <= FingerSlots.Count && (FingerSlots[nextIndex] == null || !FingerSlots[nextIndex].HasMaterial));

            if (FingerSlots[nextIndex] == null || !FingerSlots[nextIndex].HasMaterial) return;

            selectedFingerIndex = nextIndex;

            if (selectedFingerIndex != previousIndex)
                OnSelectionChanged?.Invoke(selectedFingerIndex);

            OnInventoryChanged?.Invoke();
        }
    }
}
