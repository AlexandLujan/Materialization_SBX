using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Inventory
{
    [CreateAssetMenu(menuName = "Game/Material Definition")]
    public class MaterialDefinition : ScriptableObject
    {
        public string materialName;
        public Sprite icon;
        public InventoryCategory category;
        public int maxStock;
    }
}
