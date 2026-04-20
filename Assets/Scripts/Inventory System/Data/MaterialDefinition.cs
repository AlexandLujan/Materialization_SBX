using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Materialization.Features.Inventory
{
    [CreateAssetMenu(menuName = "Game/Material Definition")]
    public class MaterialDefinition : ScriptableObject
    {
        [SerializeField] private string materialName;
        [SerializeField] private Sprite icon;
        [SerializeField] private int maxStock = 1;

        public string MaterialName => materialName;
        public Sprite Icon => icon;
        public int MaxStock => maxStock;
    }
}
