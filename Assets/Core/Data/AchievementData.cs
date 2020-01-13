using MC.Core.Interface;
using OPS.AntiCheat.Field;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Core
{

    public class AchievementData : ScriptableObject, IFileLoad, IFileSave
    {
        public System.Action OnValueChanged;

        public List<InventoryStorage> inventoryStorageList = new List<InventoryStorage>();

        public List<InventoryStorage> craftStorageList = new List<InventoryStorage>();

        public List<UserMapStorage> userMapStorageList = new List<UserMapStorage>();

        public ProtectedInt32 Coin { get => coin; set { coin = value; OnValueChanged?.Invoke(); } }
        public ProtectedInt32 Land { get => land; set { land = value; OnValueChanged?.Invoke(); } }

        public bool isBiliBiliFollower = false;

        public bool isNewPlayer = true;

        [SerializeField]
        private ProtectedInt32 coin = 0;

        [SerializeField]
        private ProtectedInt32 land = 3;

        public bool AddInv(string invName, int count)
        {
            var inv = InventoryManager.Instance.GetInventoryByName(invName);

            for (var i = 0; i < InventorySystem.max_bottom_slot_count; i++)
            {
                var index = inventoryStorageList.FindIndex(val => val.slotID == i);

                if (index == -1)
                {
                    inventoryStorageList.Add(new InventoryStorage()
                    {
                        count = count,
                        inventory = inv,
                        inventoryName = invName,
                        slotID = i
                    });

                    return true;
                }
            }

            for (var i = 0; i < InventorySystem.max_slot_count; i++)
            {
                var index = craftStorageList.FindIndex(val => val.slotID == i);

                if (index == -1)
                {
                    inventoryStorageList.Add(new InventoryStorage()
                    {
                        count = count,
                        inventory = inv,
                        inventoryName = invName,
                        slotID = i
                    });

                    return true;
                }
            }

            return false;
        }

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

            if (isNewPlayer)
            {
                isNewPlayer = false;

                AddInv("Oak Wood", 64);
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
