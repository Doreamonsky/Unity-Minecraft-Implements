using MC.Core.Interface;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MC.Core
{
    public class InventorySystem : MonoBehaviour
    {
        private const int max_item_count_per_slot = 256;
        public static int max_slot_count = 35;

        public static int max_bottom_slot_count = 10;

        //Inventory UI
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

            public GameObject ItemTemplate;

            public GameObject StorageBar;

            public List<ItemUI> itemInstance = new List<ItemUI>();

            public GameObject digProgressBar;

            public Slider digProgressSlider;

            public Text digProgressText;

            public Text currentItemText;

            public Button dropInvBtn;

            public void Init()
            {
                for (var i = 0; i < max_slot_count; i++)
                {
                    var currentIndex = i;

                    var parent = i < max_bottom_slot_count ? ItemTemplate.transform.parent : StorageBar.transform;

                    var instance = Instantiate(ItemTemplate, parent, true);

                    var uiItem = new ItemUI()
                    {
                        instance = instance,
                        itemIcon = instance.transform.Find("ItemIcon").GetComponent<Image>(),
                        itemCount = instance.transform.Find("Num").GetComponent<Text>(),
                        selectedIcon = instance.transform.Find("Selected").gameObject
                    };
                    itemInstance.Add(uiItem);

                    var iconUI = instance.AddComponent<InventoryIconUI>();
                    iconUI.Init(i, uiItem.selectedIcon, uiItem.itemIcon, InventoryIconType.Inv);

                    uiItem.instance.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        ControlEvents.OnClickInventoryByID?.Invoke(currentIndex);
                    });
                }

                ItemTemplate.gameObject.SetActive(false);
            }
        }

        public UILayout layout = new UILayout();

        //玩家拥有物品的储存
        //Slot ID 插槽  <= max_inventory_count 显示在底部
        public List<InventoryStorage> inventoryStorageList = new List<InventoryStorage>();

        //当前选用的Inventory 
        /// <summary>
        /// 注意是SlotID  inventoryStorageList.Find(val => val.slotID == currentSelectID); 方法获得Inventory
        /// </summary>
        public int currentSelectID = 0;

        public Camera playerCamera;

        //武器插槽
        public Transform weaponSlot;

        public Player player;

        //与Block交互的数据
        private float interactTime = 0;

        private Vector3 interactPos = Vector3.zero;

        private int toDropSlotID = 0;

        private Inventory currentInv;

        private void Start()
        {
#if UNITY_EDITOR

            foreach (var storage in inventoryStorageList)
            {
                storage.inventory = Instantiate(storage.inventory);
            }
#endif
            layout.Init();

            layout.dropInvBtn.onClick.AddListener(DropCurrentInv);

            SwapManager.OnHighlightedItem += (InventoryIconUI invUI) =>
            {
                if (invUI.m_iconType == InventoryIconType.Inv)
                {
                    toDropSlotID = invUI.m_slotID;
                    UpdateCurrentSelectText(invUI.m_slotID);
                }
            };

            //选择 Inventory
            ControlEvents.OnClickInventoryByID += id =>
            {
                if (!Util.isCrafting)
                {
                    SelectInventoryByID(id);
                }
            };

            ControlEvents.OnClickScreen += pos =>
            {
                //使用 Inventory （PC 右键 移动端点击屏幕）
                var isPlaceable = inventoryStorageList.Find(val => val.slotID == currentSelectID)?.inventory is IPlaceable;

                if (isPlaceable)
                {
                    PlaceCurrentInventory(pos);
                }
            };

            ControlEvents.OnBeginPressScreen += () =>
            {
                interactTime = 0;
            };

            ControlEvents.OnPressingScreen += pos =>
            {

                //与方块交互
                InteractBlock(pos);
            };

            ControlEvents.OnEndPressScreen += () =>
            {
                interactTime = 0;

                layout.digProgressBar.SetActive(false);
            };

            //分配物体
            SwapManager.OnAllocateItem += (a, b, type) =>
            {
                if (type == SwapType.CraftToCraft)
                {
                    var itemA = CraftSystem.Instance.craftInventoryList.Find(val => val.slotID == a);
                    var itemB = CraftSystem.Instance.craftInventoryList.Find(val => val.slotID == b);

                    if (itemA == null)
                    {
                        return;
                    }

                    if (itemB != null)
                    {
                        return;
                    }

                    itemA.count -= 1;

                    CraftSystem.Instance.craftInventoryList.Add(new InventoryStorage()
                    {
                        count = 1,
                        inventory = itemA.inventory,
                        slotID = b
                    });

                }
                else if (type == SwapType.InvToCraft)
                {
                    var itemA = inventoryStorageList.Find(val => val.slotID == a);
                    var itemB = CraftSystem.Instance.craftInventoryList.Find(val => val.slotID == b);

                    if (itemA == null)
                    {
                        return;
                    }

                    if (itemB != null)
                    {
                        return;
                    }

                    itemA.count -= 1;

                    CraftSystem.Instance.craftInventoryList.Add(new InventoryStorage()
                    {
                        count = 1,
                        inventory = itemA.inventory,
                        slotID = b
                    });
                }

                CleanUpInventory();
                UpdateInvetoryUI();

                CraftSystem.Instance.UpdateInvetoryUI();
            };

            //交换物体
            SwapManager.OnSwapItem += (a, b, type) =>
            {
                //Inventory 中交换物品
                if (type == SwapType.InvToInv)
                {
                    var itemA = inventoryStorageList.Find(val => val.slotID == a);
                    var itemB = inventoryStorageList.Find(val => val.slotID == b);

                    var isAppendCount = false;

                    //数量叠加
                    if (itemA != null && itemB != null)
                    {
                        if (itemA.inventory.inventoryName == itemB.inventory.inventoryName)
                        {
                            isAppendCount = true;
                        }
                    }
                    //纯粹交换物品
                    else
                    {
                        isAppendCount = false;
                    }

                    if (isAppendCount)
                    {
                        var sum = itemA.count + itemB.count;

                        if (sum > max_item_count_per_slot)
                        {
                            itemA.count = sum - max_item_count_per_slot;
                            itemB.count = max_item_count_per_slot;
                        }
                        else
                        {
                            itemB.count = itemA.count + itemB.count;
                            itemA.count = 0;
                        }
                        CleanUpInventory();
                    }
                    else
                    {
                        if (itemA != null)
                        {
                            itemA.slotID = b;
                        }

                        if (itemB != null)
                        {
                            itemB.slotID = a;
                        }
                    }

                    UpdateInvetoryUI();
                }

                //Inventory Craft 交换物品
                else if (type == SwapType.InvToCraft)
                {
                    var itemA = inventoryStorageList.Find(val => val.slotID == a);
                    var itemB = CraftSystem.Instance.craftInventoryList.Find(val => val.slotID == b);

                    var isAppendCount = false;

                    //数量叠加
                    if (itemA != null && itemB != null)
                    {
                        if (itemA.inventory.inventoryName == itemB.inventory.inventoryName)
                        {
                            isAppendCount = true;
                        }
                    }
                    //纯粹交换物品
                    else
                    {
                        isAppendCount = false;
                    }

                    if (isAppendCount)
                    {
                        var sum = itemA.count + itemB.count;

                        if (sum > max_item_count_per_slot)
                        {
                            itemA.count = sum - max_item_count_per_slot;
                            itemB.count = max_item_count_per_slot;
                        }
                        else
                        {
                            itemB.count = itemA.count + itemB.count;
                            itemA.count = 0;
                        }
                        CleanUpInventory();
                    }
                    else
                    {

                        if (itemA != null)
                        {
                            itemA.slotID = b;

                            inventoryStorageList.Remove(itemA);
                            CraftSystem.Instance.craftInventoryList.Add(itemA);
                        }

                        if (itemB != null)
                        {
                            itemB.slotID = a;

                            inventoryStorageList.Add(itemB);
                            CraftSystem.Instance.craftInventoryList.Remove(itemB);

                        }
                    }

                    UpdateInvetoryUI();
                    CraftSystem.Instance.UpdateInvetoryUI();
                }

                //Craft Inventory 交换物品
                else if (type == SwapType.CraftToInv)
                {
                    var itemA = CraftSystem.Instance.craftInventoryList.Find(val => val.slotID == a);
                    var itemB = inventoryStorageList.Find(val => val.slotID == b);

                    var isAppendCount = false;

                    //数量叠加
                    if (itemA != null && itemB != null)
                    {
                        if (itemA.inventory.inventoryName == itemB.inventory.inventoryName)
                        {
                            isAppendCount = true;
                        }
                    }
                    //纯粹交换物品
                    else
                    {
                        isAppendCount = false;
                    }

                    if (isAppendCount)
                    {
                        var sum = itemA.count + itemB.count;

                        if (sum > max_item_count_per_slot)
                        {
                            itemA.count = sum - max_item_count_per_slot;
                            itemB.count = max_item_count_per_slot;
                        }
                        else
                        {
                            itemB.count = itemA.count + itemB.count;
                            itemA.count = 0;
                        }

                        CleanUpInventory();

                    }
                    else
                    {
                        if (itemA != null)
                        {
                            itemA.slotID = b;

                            inventoryStorageList.Add(itemA);
                            CraftSystem.Instance.craftInventoryList.Remove(itemA);
                        }

                        if (itemB != null)
                        {
                            itemB.slotID = a;

                            inventoryStorageList.Remove(itemB);
                            CraftSystem.Instance.craftInventoryList.Add(itemB);
                        }
                    }

                    UpdateInvetoryUI();
                    CraftSystem.Instance.UpdateInvetoryUI();
                }

                //Craft 中交换物品
                else if (type == SwapType.CraftToCraft)
                {
                    var itemA = CraftSystem.Instance.craftInventoryList.Find(val => val.slotID == a);
                    var itemB = CraftSystem.Instance.craftInventoryList.Find(val => val.slotID == b);

                    var isAppendCount = false;

                    //数量叠加
                    if (itemA != null && itemB != null)
                    {
                        if (itemA.inventory.inventoryName == itemB.inventory.inventoryName)
                        {
                            isAppendCount = true;
                        }
                    }
                    //纯粹交换物品
                    else
                    {
                        isAppendCount = false;
                    }

                    if (isAppendCount)
                    {
                        var sum = itemA.count + itemB.count;

                        if (sum > max_item_count_per_slot)
                        {
                            itemA.count = sum - max_item_count_per_slot;
                            itemB.count = max_item_count_per_slot;
                        }
                        else
                        {
                            itemB.count = itemA.count + itemB.count;
                            itemA.count = 0;
                        }

                        CleanUpInventory();

                    }
                    else
                    {
                        if (itemA != null)
                        {
                            itemA.slotID = b;
                        }

                        if (itemB != null)
                        {
                            itemB.slotID = a;
                        }
                    }

                    CraftSystem.Instance.UpdateInvetoryUI();
                }
                //Craft 生成物体
                else if (type == SwapType.CraftedToInv)
                {
                    //当前没有合成的物体
                    if (CraftSystem.Instance.craftedInventory == null)
                    {
                        return;
                    }

                    var targetInv = inventoryStorageList.Find(val => val.slotID == b);

                    //深复制防止多引用
                    var craftedInventory = CraftSystem.Instance.craftedInventory.Clone();

                    var isCrafted = false;

                    //目标插槽空
                    if (targetInv == null)
                    {
                        isCrafted = true;
                        craftedInventory.slotID = b;

                        inventoryStorageList.Add(craftedInventory);
                    }
                    else if (targetInv.inventory.inventoryName == craftedInventory.inventory.inventoryName && targetInv.count + craftedInventory.count <= max_item_count_per_slot)
                    {
                        isCrafted = true;
                        craftedInventory.slotID = b;

                        targetInv.count += craftedInventory.count;
                    }

                    //满足Craft 条件后，销毁原材料
                    if (isCrafted)
                    {
                        foreach (var usedItem in CraftSystem.Instance.craftInventoryList)
                        {
                            usedItem.count -= 1;
                        }

                        CleanUpInventory();
                        UpdateInvetoryUI();
                        CraftSystem.Instance.UpdateInvetoryUI();
                    }
                }
            };

            UpdateInvetoryUI();

            SelectInventoryByID(currentSelectID);
        }

        private void CleanUpInventory()
        {
            //清除无效的物品
            for (int i = inventoryStorageList.Count - 1; i >= 0; i--)
            {
                var item = inventoryStorageList[i];

                if (item.count <= 0)
                {
                    inventoryStorageList.Remove(item);
                }
            }

            for (int i = CraftSystem.Instance.craftInventoryList.Count - 1; i >= 0; i--)
            {
                var item = CraftSystem.Instance.craftInventoryList[i];

                if (item.count <= 0)
                {
                    CraftSystem.Instance.craftInventoryList.Remove(item);
                }
            }
        }

        public void UpdateInvetoryUI()
        {
            //更新底部插槽UI
            for (var i = 0; i < max_slot_count; i++)
            {
                var item = inventoryStorageList.Find(val => val?.slotID == i);

                if (item != null)
                {
                    layout.itemInstance[i].itemIcon.sprite = item.inventory?.inventoryIcon;
                    layout.itemInstance[i].itemCount.text = item.count.ToString();
                }
                else
                {
                    layout.itemInstance[i].itemIcon.sprite = null;
                    layout.itemInstance[i].itemCount.text = "0";
                }

                if (!Util.isCrafting)
                {
                    if (currentSelectID == i)
                    {
                        layout.itemInstance[i].selectedIcon.SetActive(true);
                    }
                    else
                    {
                        layout.itemInstance[i].selectedIcon.SetActive(false);
                    }
                }
            }
        }

        public void SelectInventoryByID(int id)
        {
            //取消之前的选择
            currentInv?.OnUnselected(this);

            //更新
            currentSelectID = id;
            UpdateInvetoryUI();

            //选择新的物体
            currentInv = inventoryStorageList.Find(val => val.slotID == currentSelectID)?.inventory;
            currentInv?.OnSelected(this);

            toDropSlotID = currentSelectID;
            UpdateCurrentSelectText(currentSelectID);
        }

        private void UpdateCurrentSelectText(int slotID)
        {
            var currInv = inventoryStorageList.Find(val => val.slotID == slotID)?.inventory;

            layout.currentItemText.text = $"当前选择的物品: { currInv?.inventoryName}";
        }

        private void PlaceCurrentInventory(Vector2 screenPos)
        {
            var currentStorage = inventoryStorageList.Find(val => val.slotID == currentSelectID);

            //当前物品为空
            if (currentStorage == null)
            {
                UpdateInvetoryUI();
                return;
            }

            //物品数量不足
            if (currentStorage.count <= 0)
            {
                return;
            }

            var ray = playerCamera.ScreenPointToRay(screenPos);
            var isHit = Physics.Raycast(ray, out RaycastHit rayHit, 240, 1 << LayerMask.NameToLayer("Block") | 1 << LayerMask.NameToLayer("Item"));

            var currInv = currentStorage.inventory;

            //可放置的物体
            if (currInv is PlaceableInventory)
            {
                var inv = (PlaceableInventory)currInv;

                if (isHit)
                {
                    var worldManager = rayHit.collider.GetComponentInParent<WorldManager>();
                    var worldPlacePoint = rayHit.point + rayHit.normal * 0.5f * WorldManager.scaleSize;
                    var localPlacePoint = worldManager.transform.InverseTransformPoint(worldPlacePoint);

                    var height = Mathf.FloorToInt(localPlacePoint.y / WorldManager.scaleSize);
                    var x = Mathf.FloorToInt(localPlacePoint.x / WorldManager.scaleSize);
                    var y = Mathf.FloorToInt(localPlacePoint.z / WorldManager.scaleSize);

                    var isUsed = inv.Place(worldManager, new Vector3Int(x, height, y), ray.direction);

                    if (isUsed)
                    {
                        CurrentInventroyUsed(currentStorage);
                    }
                }

            }

        }

        private void CurrentInventroyUsed(InventoryStorage currentStorage)
        {
            currentStorage.count -= 1;
            CleanUpInventory();
            UpdateInvetoryUI();
        }

        private void InteractBlock(Vector2 screenPos)
        {
            var currentInv = inventoryStorageList.Find(val => val.slotID == currentSelectID)?.inventory;

            var digSpeedBoost = 1f;

            if (currentInv is IDigBoost)
            {
                var digboost = currentInv as IDigBoost;
                digSpeedBoost = digboost.GetDigBoost();
            }

            if (currentInv is IAttackable)
            {
                var attackable = currentInv as IAttackable;
                attackable.Attack(player);
            }

            digSpeedBoost = 1f / digSpeedBoost;

            var ray = playerCamera.ScreenPointToRay(screenPos);
            var isHit = Physics.Raycast(ray, out RaycastHit rayHit, 240, 1 << LayerMask.NameToLayer("Block") | 1 << LayerMask.NameToLayer("Item"));

            if (isHit)
            {
                var worldManager = rayHit.collider.GetComponentInParent<WorldManager>();
                var worldPlacePoint = rayHit.point - rayHit.normal * 0.5f * WorldManager.scaleSize;
                var localPlacePoint = worldManager.transform.InverseTransformPoint(worldPlacePoint);

                var height = Mathf.FloorToInt(localPlacePoint.y / WorldManager.scaleSize);
                var x = Mathf.FloorToInt(localPlacePoint.x / WorldManager.scaleSize);
                var y = Mathf.FloorToInt(localPlacePoint.z / WorldManager.scaleSize);

                var chunckPoint = new Vector3(x, height, y);

                IDigable digableItem = null;

                RuntimePlaceableInventoryData itemData = null;

                var isBlock = false;
                var isItem = false;

                if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Block"))
                {
                    isBlock = true;

                    var blockData = worldManager.GetBlockData(height, x, y);
                    digableItem = blockData as IDigable;
                }

                if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Item"))
                {
                    isItem = true;

                    itemData = worldManager.GetItemData(new Vector3Int(x, height, y));
                    digableItem = itemData.inventory as IDigable;
                }

                if (digableItem != null)
                {
                    //判断 当前挖的与上一帧挖的是否相同
                    if (interactPos == chunckPoint)
                    {
                        if (interactTime < digableItem.DigTime())
                        {
                            interactTime += Time.deltaTime;
                        }
                    }
                    else
                    {
                        interactPos = chunckPoint;
                        interactTime = 0;
                    }

                    if (interactTime >= digableItem.DigTime() * digSpeedBoost && interactPos == chunckPoint)
                    {
                        if (isBlock)
                        {
                            worldManager.DeleteBlock((int)chunckPoint.y, (int)chunckPoint.x, (int)chunckPoint.z);
                        }
                        else if (isItem)
                        {
                            worldManager.RemoveInventroy(itemData);
                        }

                        digableItem.DropInventory(chunckPoint);
                    }

                    //Update UI
                    if (interactTime > 0.15f)
                    {
                        layout.digProgressBar.SetActive(true);

                        layout.digProgressSlider.value = interactTime / (digableItem.DigTime() * digSpeedBoost);
                        layout.digProgressText.text = $"{((interactTime / (digableItem.DigTime() * digSpeedBoost)) * 100).ToString("f1")} %";

                        SoundManager.Instance.PlayDigSound(digableItem.DigSound());
                    }
                    else
                    {
                        layout.digProgressBar.SetActive(false);
                    }

                }
                else
                {
                    layout.digProgressBar.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Ops,noting is hit");
            }
        }

        public void AddStorage(InventoryStorage inventoryStorage)
        {
            inventoryStorageList.Add(inventoryStorage);
            UpdateInvetoryUI();
        }

        public void DropCurrentInv()
        {
            var currentInv = inventoryStorageList.Find(val => val.slotID == toDropSlotID);

            if (currentInv != null)
            {
                inventoryStorageList.Remove(currentInv);
            }

            UpdateInvetoryUI();
        }
    }

}