using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Inventory.UI
{
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;

        private void Awake()
        {
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
        }

        public void SetSlotVisual(Vector2 position, Vector2 size, Vector3 scale, int siblingIndex)
        {
            rectTransform.anchoredPosition = position;
            rectTransform.sizeDelta = size;
            rectTransform.localScale = scale;
            rectTransform.SetSiblingIndex(siblingIndex);
        }
    }
}
