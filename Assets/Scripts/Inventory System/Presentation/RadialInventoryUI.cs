using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Inventory.UI
{
    public class RadialInventoryUI : MonoBehaviour
    {
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private InventorySlotView palmView;
        [SerializeField] private InventorySlotView[] fingerViews;

        private void OnEnable()
        {
            if (inventorySystem == null)
                return;

            inventorySystem.OnInventoryChanged += RefreshUI;
            inventorySystem.OnSelectionChanged += HandleSelectionChanged;

            RefreshUI();
        }

        private void OnDisable()
        {
            if (inventorySystem == null)
                return;

            inventorySystem.OnInventoryChanged -= RefreshUI;
            inventorySystem.OnSelectionChanged -= HandleSelectionChanged;

            RefreshUI();
        }

        private void HandleSelectionChanged(int index)
        {
            RefreshUI();
        }

        private void RefreshUI()
        {
            RefreshPalm();
            RefreshFingers();
        }

        private void RefreshPalm()
        {
            InventorySlot slot = inventorySystem.EquippedSlot;

            if (slot == null || slot.IsEmpty)
                palmView.SetEmpty();
            else
                palmView.SetFilled(slot.materialData.icon, slot.currentStock);

            palmView.SetSelected(false);
        }

        private void RefreshFingers()
        {
            List<InventorySlot> slots = inventorySystem.FingerSlots;

            for (int i = 0; i < fingerViews.Length; i++)
            {
                if (slots == null || i >= slots.Count || slots[i] == null || slots[i].IsEmpty)
                    fingerViews[i].SetEmpty();
                else
                    fingerViews[i].SetFilled(slots[i].materialData.icon, slots[i].currentStock);

                fingerViews[i].SetSelected(i == inventorySystem.SelectedFingerIndex);
            }
        }
    }
}
