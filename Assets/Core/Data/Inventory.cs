using UnityEngine;

namespace MC.Core
{
    public abstract class Inventory : ScriptableObject
    {
        public string inventoryName;

        public Sprite inventoryIcon;

        public abstract void OnSelected(InventorySystem inventorySystem);

        public abstract void OnUnselected(InventorySystem inventorySystem);
    }

}
