using System.Collections.Generic;
using UnityEngine;

namespace MC.Core
{
    public class AchievementData : ScriptableObject
    {
        [System.Serializable]
        public struct InventorySaving
        {
            public string name;
            public int slotID, count;
        }

        public List<InventorySaving> inventoryStorageList = new List<InventorySaving>();
    }
}
