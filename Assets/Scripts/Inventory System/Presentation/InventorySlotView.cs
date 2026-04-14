using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Materialization.Features.Inventory.UI
{
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image placeholderImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image highlightImage;
        [SerializeField] private TMP_Text stockText;

        private void Awake()
        {
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
        }

        public void SetEmpty()
        {
            if (placeholderImage != null)
                placeholderImage.enabled = true;

            if (iconImage != null)
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
            }

            if (stockText != null)
                stockText.text = "";

            SetSelected(false);
        }

        public void SetFilled(Sprite icon, int stock)
        {
            if (placeholderImage != null)
                placeholderImage.enabled = true;

            if (iconImage != null)
            {
                iconImage.sprite = icon;
                iconImage.enabled = icon != null;
            }

            if (stockText != null)
                stockText.text = stock.ToString();
        }

        public void SetSelected(bool selected)
        {
            if (highlightImage != null)
                highlightImage.enabled = selected;
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
