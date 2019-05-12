using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "Inventory")]
    public class Inventory : ScriptableObject
    {
        public string inventoryName;

        public Sprite inventoryIcon;

        [Header("Block Data:")]
        public bool isBlockItem;

        public BlockData blockData;
    }

}
