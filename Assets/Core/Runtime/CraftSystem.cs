using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MC.Core
{
    public class CraftSystem : MonoBehaviour
    {
        [System.Serializable]
        public class UILayout
        {
            public class ItemUI
            {
                public Image itemIcon;

                public GameObject instance;
            }

            public List<GameObject> craftObject = new List<GameObject>();

            public List<ItemUI> itemInstance = new List<ItemUI>();

            public void Init()
            {
                for (int i = 0; i < craftObject.Count; i++)
                {
                    var item = craftObject[i];

                    var itemUI = new ItemUI()
                    {
                        instance = item,
                        itemIcon = item.transform.Find("ItemIcon").GetComponent<Image>()
                    };

                    itemInstance.Add(itemUI);

                    var iconUI = item.AddComponent<InventoryIconUI>();
                    iconUI.Init(i, itemUI.itemIcon, false);
                }
            }
        }

        public List<InventoryStorage> craftInventoryList = new List<InventoryStorage>();

        public List<RecipeData> recipeList = new List<RecipeData>();

        public UILayout layout = new UILayout();

        public static CraftSystem Instance;

        private void Start()
        {
            Instance = this;

            layout.Init();
        }

        public void UpdateInvetoryUI()
        {
            //更新插槽 Icon
            for (var i = 0; i < 9; i++)
            {
                var item = craftInventoryList.Find(val => val?.slotID == i);

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

        public void GuessRecipe()
        {
            foreach (var recipe in recipeList)
            {

            }
        }
    }

}
