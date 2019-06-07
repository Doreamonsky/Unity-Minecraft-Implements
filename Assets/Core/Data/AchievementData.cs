using MC.Core.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Core
{
    public class AchievementData : ScriptableObject, IFileLoad, IFileSave
    {
        public List<InventoryStorage> inventoryStorageList = new List<InventoryStorage>();

        public void OnLoad()
        {
            foreach (var p in inventoryStorageList)
            {
                p.OnLoad();
            }
        }

        public void OnSave()
        {
            foreach (var p in inventoryStorageList)
            {
                p.OnSave();
            }
        }
    }
}
