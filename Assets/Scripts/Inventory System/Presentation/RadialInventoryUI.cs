using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Inventory.UI
{
    public class RadialInventoryUI : MonoBehaviour
    {
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private GameObject root;

        [Header("Views")]
        [SerializeField] private InventorySlotView palmView;
        [SerializeField] private InventorySlotView[] fingerViews;

        private void Awake()
        {
            if (inventorySystem == null)
                inventorySystem = GetComponentInParent<InventorySystem>();

            if (root != null)
                root.SetActive(false);
        }

        private void OnEnable()
        {
            if (inventorySystem == null)
                return;

            inventorySystem.OnInventoryChanged += RefreshUI;
            inventorySystem.OnSelectionChanged += HandleSelectionChanged;
        }

        private void OnDisable()
        {
            if (inventorySystem == null)
                return;

            inventorySystem.OnInventoryChanged -= RefreshUI;
            inventorySystem.OnSelectionChanged -= HandleSelectionChanged;
        }

        private void Start()
        {
            RefreshUI();
        }

        private void HandleSelectionChanged(int index)
        {
            RefreshUI();
        }

        private void RefreshUI()
        {
            if (inventorySystem == null)
            {
                Debug.LogWarning("RadialInventoryUI: inventorySystem is null.");
                return;
            }

            Debug.Log($"RadialInventoryUI RefreshUI -> IsOpen: {inventorySystem.IsOpen}");

            if (root != null)
                root.SetActive(inventorySystem.IsOpen);

            RefreshPalm();
            RefreshFingers();
        }

        private void RefreshPalm()
        {
            if (palmView == null)
                return;

            palmView.Refresh(
                inventorySystem.EquippedSlot,
                isSelected: false,
                isFingerSlot: false
            );
        }

        private void RefreshFingers()
        {
            List<InventorySlot> slots = inventorySystem.FingerSlots;

            for (int i = 0; i < fingerViews.Length; i++)
            {
                if (fingerViews[i] == null) continue;

                if (slots == null || i >= slots.Count || slots[i] == null || slots[i].IsEmpty || slots[i] == null)
                {
                    fingerViews[i].SetEmpty();
                    continue;
                }

                bool isSelected = i == inventorySystem.SelectedFingerIndex;

                fingerViews[i].Refresh(
                    slots[i],
                    isSelected: isSelected,
                    isFingerSlot: true
                );
            }
        }
    }
}
