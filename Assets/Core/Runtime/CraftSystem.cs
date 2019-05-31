using System.Collections.Generic;
using System.Linq;
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

                public Text itemCount;

                public GameObject selectedIcon;
            }

            public GameObject craftedObject;

            public List<GameObject> craftObject = new List<GameObject>();

            public ItemUI craftedInstance = new ItemUI();

            public List<ItemUI> itemInstance = new List<ItemUI>();

            public void Init()
            {
                for (int i = 0; i < craftObject.Count; i++)
                {
                    var item = craftObject[i];

                    Debug.Log(i);

                    var itemUI = new ItemUI()
                    {
                        instance = item,
                        itemCount = item.transform.Find("Num").GetComponent<Text>(),
                        itemIcon = item.transform.Find("ItemIcon").GetComponent<Image>(),
                        selectedIcon = item.transform.Find("Selected").gameObject
                    };

                    itemInstance.Add(itemUI);

                    var iconUI = item.AddComponent<InventoryIconUI>();
                    iconUI.Init(i, itemUI.selectedIcon, itemUI.itemIcon, InventoryIconType.Craft);
                }

                craftedInstance.instance = craftedObject;
                craftedInstance.itemIcon = craftedObject.transform.Find("ItemIcon").GetComponent<Image>();
                craftedInstance.selectedIcon = craftedObject.transform.Find("Selected").gameObject;

                var craftedIconUI = craftedObject.AddComponent<InventoryIconUI>();
                craftedIconUI.Init(0, craftedInstance.selectedIcon, craftedInstance.itemIcon, InventoryIconType.Crafted);
            }
        }

        public List<InventoryStorage> craftInventoryList = new List<InventoryStorage>();

        public List<RecipeData> recipeList = new List<RecipeData>();

        public UILayout layout = new UILayout();

        public InventoryStorage craftedInventory;

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
                    layout.itemInstance[i].itemCount.text = item.count.ToString();
                }
                else
                {
                    layout.itemInstance[i].itemIcon.sprite = null;
                    layout.itemInstance[i].itemCount.text = "0";
                }
            }

            GuessRecipe();
        }

        public void GuessRecipe()
        {
            //匹配Recipe的字符组合
            var currentRecipe = new string[9] { "", "", "", "", "", "", "", "", "" };

            RecipeData desireRecipeData = null;

            foreach (var craft in craftInventoryList)
            {
                currentRecipe[craft.slotID] = craft.inventory.inventoryName;
            }


            foreach (var recipe in recipeList)
            {
                var targetRecipe = new string[9] { "", "", "", "", "", "", "", "", "" };

                for (int i = 0; i < recipe.Recipe.Length; i++)
                {
                    targetRecipe[i] = recipe.Recipe[i] == null ? "" : recipe.Recipe[i].inventoryName;
                }

                if (Enumerable.SequenceEqual(currentRecipe, targetRecipe))
                {
                    desireRecipeData = recipe;
                    break;
                }
                else
                {
                    //Debug.Log($"current:{JsonConvert.SerializeObject(currentRecipe)} target:{JsonConvert.SerializeObject(targetRecipe)}");
                }
            }

            if (desireRecipeData != null)
            {
                Debug.Log(desireRecipeData);

                layout.craftedInstance.itemIcon.sprite = desireRecipeData.CraftedInventory.inventoryIcon;

                craftedInventory = new InventoryStorage()
                {
                    count = desireRecipeData.CraftedCount,
                    slotID = 0,
                    inventory = desireRecipeData.CraftedInventory
                };
            }
            else
            {
                layout.craftedInstance.itemIcon.sprite = null;
                craftedInventory = null;
            }
        }
    }

}
