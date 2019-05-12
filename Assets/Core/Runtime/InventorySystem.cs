using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MC.Core
{
    public class InventorySystem : MonoBehaviour
    {
        public static int max_inventory_count = 10;

        //Inventory UI
        [System.Serializable]
        public class UILayout
        {
            public class ItemUI
            {
                public Image itemIcon;

                public GameObject instance;
            }

            public GameObject ItemTemplate;

            public List<ItemUI> itemInstance = new List<ItemUI>();

            public void Init()
            {
                for (var i = 0; i < max_inventory_count; i++)
                {
                    var instance = Object.Instantiate(ItemTemplate, ItemTemplate.transform.parent, true);

                    var uiItem = new ItemUI()
                    {
                        instance = instance,
                        itemIcon = instance.transform.Find("ItemIcon").GetComponent<Image>()
                    };
                    itemInstance.Add(uiItem);

                    var iconUI = instance.AddComponent<InventoryIconUI>();
                    iconUI.Init(i, uiItem.itemIcon);
                }

                ItemTemplate.gameObject.SetActive(false);
            }
        }


        //玩家拥有物品的储存
        //Slot ID 插槽  <= max_inventory_count 显示在底部
        [System.Serializable]
        public class InventoryStorage
        {
            public int count = 1;
            public int slotID = -1;
            public Inventory inventory;
        }

        public UILayout layout = new UILayout();

        public List<InventoryStorage> inventoryStorageList = new List<InventoryStorage>();

        private void Start()
        {
            layout.Init();

            //交换物品
            InventoryIconUI.OnSwapItem += (a, b) =>
            {
                var itemA = inventoryStorageList.Find(val => val.slotID == a);
                var itemB = inventoryStorageList.Find(val => val.slotID == b);

                if (itemA != null)
                {
                    itemA.slotID = b;
                }

                if (itemB != null)
                {
                    itemB.slotID = a;
                }

                UpdateInvetoryUI();
            };

            UpdateInvetoryUI();
        }

        private void UpdateInvetoryUI()
        {
            //更新底部插槽
            for (var i = 0; i < max_inventory_count; i++)
            {
                var item = inventoryStorageList.Find(val => val.slotID == i);

                if (item != null)
                {
                    layout.itemInstance[i].itemIcon.sprite = item.inventory.inventoryIcon;
                }
                else
                {
                    layout.itemInstance[i].itemIcon.sprite = null;
                }
            }
        }

    }

}
