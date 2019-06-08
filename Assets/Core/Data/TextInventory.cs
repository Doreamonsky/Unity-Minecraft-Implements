using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Core {
    [CreateAssetMenu (fileName = "TextInventory")]
    public class TextInventory : Inventory {
        public string text;
        public override void OnSelected (InventorySystem inventorySystem) {
            inventorySystem.player.textInfomation.text = text;
        }

        public override void OnUnselected (InventorySystem inventorySystem) {
            inventorySystem.player.textInfomation.text = null;
        }
    }

}