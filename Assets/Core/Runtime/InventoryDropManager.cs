
using UnityEngine;

namespace MC.Core
{
    public class InventoryDropManager : MonoBehaviour
    {
        public static InventoryDropManager Instance;

        public InventorySystem inventorySystem;

        public GameObject dropPrefab;

        private void Start()
        {
            Instance = this;
        }

        public void CreateDropBlockInventory(Vector3 pos, InventoryStorage inventoryStorage)
        {
            var isHit = Physics.Raycast(pos, Vector3.up * -1, out RaycastHit rayHit, 20, 1 << LayerMask.NameToLayer("Block"));

            var instance = Instantiate(dropPrefab, isHit ? rayHit.point + Vector3.up * 0.5f : pos, Quaternion.identity);

            var dropBlockInventory = instance.GetComponent<DropInventory>();

            if (inventoryStorage.inventory is PlaceableInventory)
            {
                var placeable = inventoryStorage.inventory as PlaceableInventory;

                if (placeable.blockData != null)
                {
                    instance.GetComponentInChildren<MeshRenderer>().material = placeable.blockData.topTex;
                }
            }

            dropBlockInventory.OnPlayerEnter += () =>
            {
                var storedItem = inventorySystem.inventoryStorageList.Find(val => val.inventory.inventoryName == inventoryStorage.inventory.inventoryName);

                var isCollected = false;

                if (storedItem != null)
                {
                    if (storedItem.count < 64)
                    {
                        storedItem.count += inventoryStorage.count;
                        isCollected = true;

                        inventorySystem.UpdateInvetoryUI();
                    }
                }
                else
                {
                    //找空闲的Slot
                    for (var i = 0; i < InventorySystem.max_bottom_inventory_count; i++)
                    {
                        var index = inventorySystem.inventoryStorageList.FindIndex(val => val.slotID == i);

                        if (index == -1)
                        {
                            inventoryStorage.slotID = i;
                            inventorySystem.AddStorage(inventoryStorage);

                            isCollected = true;
                            break;
                        }
                    }
                }

                if (isCollected)
                {
                    dropBlockInventory.Collect();
                }
            };
        }
    }
}
