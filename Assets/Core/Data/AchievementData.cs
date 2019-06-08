using MC.Core.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Core
{
    public class AchievementData : ScriptableObject, IFileLoad, IFileSave
    {
        public List<InventoryStorage> inventoryStorageList = new List<InventoryStorage>();

        public List<InventoryStorage> craftStorageList = new List<InventoryStorage>();

        public TOD_CycleParameters timeCycle;

        public Vector3 infinitePlayerPos;

        public void OnLoad()
        {
            foreach (var p in inventoryStorageList)
            {
                p.OnLoad();
            }

            foreach (var p in craftStorageList)
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

            foreach (var p in craftStorageList)
            {
                p.OnSave();
            }
        }
    }
}
