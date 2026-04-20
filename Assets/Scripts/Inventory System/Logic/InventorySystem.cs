using JetBrains.Annotations;
using Materialization.Features.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Inventory
{
    public class InventorySystem : MonoBehaviour
    {
        [SerializeField] private InventorySlot equippedSlot = new InventorySlot();
        [SerializeField] private InventoryCategory materialCategory;
        [SerializeField] private int selectedFingerIndex = 0;
        [SerializeField] private bool isOpen;
        public bool IsOpen => isOpen;

        public InventorySlot EquippedSlot => equippedSlot;
        public List<InventorySlot> FingerSlots => materialCategory.slots;
        public int SelectedFingerIndex => selectedFingerIndex;

        public event Action OnInventoryChanged;
        public event Action<int> OnSelectionChanged;

        private void Start()
        {
            OnInventoryChanged?.Invoke();
        }

        public void SelectFinger(int index)
        {
            if (materialCategory == null || materialCategory.slots == null || materialCategory.slots.Count == 0)
                return;

            if (index < 0 || index >= materialCategory.slots.Count)
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
            if (materialCategory == null || materialCategory.slots == null || materialCategory.slots.Count == 0)
                return;

            if (selectedFingerIndex < 0 || selectedFingerIndex >= materialCategory.slots.Count)
                return;

            equippedSlot.SwapWith(materialCategory.slots[selectedFingerIndex]);
            OnInventoryChanged?.Invoke();
            OnSelectionChanged?.Invoke(SelectedFingerIndex);
        }

        public void OpenInventory()
        {
            if (isOpen) return;

            isOpen = true;
            Debug.Log("[InventorySystem] Inventory Opened.");

            if (FingerSlots != null && FingerSlots.Count > 0)
            {
                if (selectedFingerIndex < 0 || selectedFingerIndex >= FingerSlots.Count)
                    selectedFingerIndex = 0;
            }

            OnInventoryChanged?.Invoke();
            OnSelectionChanged?.Invoke(selectedFingerIndex);
        }

        public void CloseInventory()
        {
            if (!isOpen) return;

            isOpen = false;
            OnInventoryChanged?.Invoke();
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
