using JetBrains.Annotations;
using Materialization.Features.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private InventorySlot equippedSlot = new InventorySlot();
    [SerializeField] private InventoryCategory materialCategory;
    [SerializeField] private int selectedFingerIndex = 0;

    public InventorySlot EquippedSlot => equippedSlot;
    public List<InventorySlot> FingerSlots => materialCategory.slots;
    public int SelectedFingerIndex => selectedFingerIndex;

    public event Action OnInventoryChanged;
    public event Action<int> OnSelectionChanged;

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
        if (materialCategory == null || materialCategory.slots == null || materialCategory.slots.Count == 0)
            return;

        selectedFingerIndex = (selectedFingerIndex + 1) % materialCategory.slots.Count;
        OnSelectionChanged?.Invoke(selectedFingerIndex);
        OnInventoryChanged?.Invoke();
    }

    public void SelectPreviousFinger()
    {
        if (materialCategory == null || materialCategory.slots == null || materialCategory.slots.Count == 0)
            return;

        selectedFingerIndex = (selectedFingerIndex - 1 + materialCategory.slots.Count) % materialCategory.slots.Count;
        OnSelectionChanged?.Invoke(selectedFingerIndex);
        OnInventoryChanged?.Invoke();
    }

    public void SwapPalmWithFinger()
    {
        if (materialCategory == null || materialCategory.slots == null || materialCategory.slots.Count == 0)
            return;

        if (selectedFingerIndex < 0 || selectedFingerIndex >= materialCategory.slots.Count)
            return;

        equippedSlot.SwapWith(materialCategory.slots[selectedFingerIndex]);
        OnInventoryChanged?.Invoke();
    }
}
