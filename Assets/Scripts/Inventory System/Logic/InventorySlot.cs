using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Inventory
{
    [System.Serializable]
    public class InventorySlot
    {
        public MaterialDefinition materialData;
        public int currentStock;
        public int maxStock;

        public string ItemName => materialData != null ? materialData.materialName : string.Empty;
        public bool IsEmpty => materialData == null || currentStock <= 0;

        public void Clear()
        {
            materialData = null;
            currentStock = 0;
            maxStock = 0;
        }

        public void SetSlot(MaterialDefinition newMaterial, int stock)
        {
            materialData = newMaterial;
            currentStock = stock;
            maxStock = newMaterial != null ? newMaterial.maxStock : 0;
        }

        public void SwapWith(InventorySlot other)
        {
            if (other == null)
                return;

            MaterialDefinition tempMaterial = materialData;
            int tempCurrentStock = currentStock;
            int tempMaxStock = maxStock;

            materialData = other.materialData;
            currentStock = other.currentStock;
            maxStock = other.maxStock;

            other.materialData = tempMaterial;
            other.currentStock = tempCurrentStock;
            other.maxStock = tempMaxStock;
        }
    }
}
