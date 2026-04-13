using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Inventory
{
    [System.Serializable]
    public class InventoryCategory
    {
        public string categoryName;
        public List<InventorySlot> slots;
    }
}
