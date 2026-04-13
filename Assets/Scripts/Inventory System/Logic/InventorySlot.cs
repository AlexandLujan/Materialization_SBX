using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Inventory
{
    [System.Serializable]
    public class InventorySlot
    {
        public string itemName;
        public int currentStock;
        public int maxStock;
    }
}
