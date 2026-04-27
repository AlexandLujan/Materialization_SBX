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
        [SerializeField] private CanvasGroup canvasGroup;

        private void Awake()
        {
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
            if (stockText == null)
                stockText = GetComponentInChildren<TMP_Text>(true);
        }

        public void Refresh(InventorySlot slot, bool isSelected, bool isFingerSlot, bool showHighlight)
        {
            bool hasMaterial = slot != null && slot.HasMaterial;

            if (!hasMaterial)
            {
                SetEmpty(isFingerSlot);
            }
            else
            {
                Sprite icon = null;

                if (slot.MaterialData != null)
                    icon = slot.MaterialData.Icon;

                SetFilled(icon, slot.CurrentStock, isFingerSlot);

                if (canvasGroup != null)
                    canvasGroup.alpha = slot.HasStock ? 1f : 0.4f;
            }

            SetSelected(showHighlight && isSelected);
        }

        public void SetEmpty(bool isFingerSlot)
        {
            if (placeholderImage != null)
                placeholderImage.enabled = true;

            if (iconImage != null)
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
            }

            if (stockText != null)
            {
                if (isFingerSlot)
                {
                    stockText.text = string.Empty;
                    stockText.gameObject.SetActive(false);
                }
                else
                {
                    stockText.text = string.Empty;
                    stockText.gameObject.SetActive(true);
                }
            }

            if (canvasGroup != null)
                canvasGroup.alpha = 1f;
        }

        public void SetFilled(Sprite icon, int stock, bool isFingerSlot)
        {
            if (placeholderImage != null)
                placeholderImage.enabled = false;

            if (iconImage != null)
            {
                iconImage.sprite = icon;
                iconImage.enabled = icon != null;
            }

            if (stockText != null)
            {
                if (isFingerSlot)
                {
                    stockText.text = string.Empty;
                    stockText.gameObject.SetActive(false);
                }
                else
                {
                    stockText.text = stock.ToString();
                    stockText.gameObject.SetActive(true);
                }
            }
        }

        public void SetSelected(bool selected)
        {
            if (highlightImage != null)
                highlightImage.enabled = selected;
        }

        public void SetSlotVisual(Vector2 position, Vector2 size, Vector3 scale, int siblingIndex)
        {
            if (rectTransform == null) return;

            rectTransform.anchoredPosition = position;
            rectTransform.sizeDelta = size;
            rectTransform.localScale = scale;
            rectTransform.SetSiblingIndex(siblingIndex);
        }
    }
}
