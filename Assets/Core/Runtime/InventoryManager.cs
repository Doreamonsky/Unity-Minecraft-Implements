using System.Collections.Generic;
using UnityEngine;

namespace MC.Core
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;

        public List<Inventory> inventories;

        private void Start()
        {
            Instance = this;
        }

        public Inventory GetInventoryByName(string name) => Instantiate(inventories.Find(val => val.inventoryName == name));

    }

}
