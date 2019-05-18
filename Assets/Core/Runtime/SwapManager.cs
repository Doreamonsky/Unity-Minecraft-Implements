using System.Collections.Generic;
using UnityEngine;

namespace MC.Core
{
    public class SwapManager : MonoBehaviour
    {
        public static System.Action<int, int, SwapType> OnSwapItem;

        public static System.Action<int, int, SwapType> OnAllocateItem;

        private List<InventoryIconUI> inventoryIcons = new List<InventoryIconUI>();

        public static SwapManager Instance;

        private InventoryIconUI prevIconUI;

        private InventoryIconUI horveredIconUI;

        public void AppendIconUI(InventoryIconUI iconUI)
        {
            inventoryIcons.Add(iconUI);
        }

        private void Update()
        {
            if (!Util.isCrafting)
            {
                return;
            }

            if (!Util.IsMobile())
            {
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    AllocateItem();
                }
            }

        }
        private void Start()
        {
            Instance = this;

            Util.OnToggleCraftingMode += (state) =>
            {
                if (state)
                {
                    ToggleIconSelection(false);
                }
            };

            InventoryIconUI.OnPointerClicked += (InventoryIconUI target) =>
            {
                //仅在创作中 需要交换物体等行为
                if (!Util.isCrafting)
                {
                    return;
                }
                if (!Util.IsMobile())
                {
                    if (!Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        return;
                    }
                }

                if (prevIconUI == null)
                {
                    prevIconUI = target;

                    foreach (var t in inventoryIcons)
                    {
                        t.ToggleSelectEffect(t == target);
                    }
                }
                else if (prevIconUI != target)
                {
                    var swapType = GetSwapType(prevIconUI, target);

                    OnSwapItem?.Invoke(prevIconUI.m_slotID, target.m_slotID, swapType);

                    Debug.Log($"Perform Swap {prevIconUI.m_slotID} {target.m_slotID} {swapType}");

                    prevIconUI = null;

                    ToggleIconSelection(false);
                }
            };

            //创作中 分配Inventory 
            InventoryIconUI.OnPointerEntered += (InventoryIconUI target) =>
             {
                 if (!Util.isCrafting)
                 {
                     return;
                 }

                 horveredIconUI = target;

                 if (Util.IsMobile())
                 {
                     AllocateItem();
                 }
             };

            InventoryIconUI.OnPointerExited += (InventoryIconUI target) =>
            {
                horveredIconUI = null;
            };
        }

        private void ToggleIconSelection(bool state)
        {
            foreach (var t in inventoryIcons)
            {
                t.ToggleSelectEffect(state);
            }
        }

        private void AllocateItem()
        {
            if (prevIconUI == null || horveredIconUI == null)
            {
                return;
            }
            var swapType = GetSwapType(prevIconUI, horveredIconUI);

            if (swapType != SwapType.InvToCraft && swapType != SwapType.CraftToCraft)
            {
                return;
            }

            OnAllocateItem?.Invoke(prevIconUI.m_slotID, horveredIconUI.m_slotID, swapType);

            Debug.Log($"Allocate Swap {prevIconUI.m_slotID} {horveredIconUI.m_slotID} {swapType}");

            horveredIconUI = null;
        }

        private SwapType GetSwapType(InventoryIconUI a, InventoryIconUI b)
        {
            var swapType = SwapType.InvToInv;

            if (a.m_iconType == InventoryIconType.Inv && b.m_iconType == InventoryIconType.Inv)
            {
                swapType = SwapType.InvToInv;
            }
            else if (a.m_iconType == InventoryIconType.Inv && b.m_iconType == InventoryIconType.Craft)
            {
                swapType = SwapType.InvToCraft;
            }
            else if (a.m_iconType == InventoryIconType.Craft && b.m_iconType == InventoryIconType.Inv)
            {
                swapType = SwapType.CraftToInv;
            }
            else if (a.m_iconType == InventoryIconType.Craft && b.m_iconType == InventoryIconType.Craft)
            {
                swapType = SwapType.CraftToCraft;
            }
            else if (a.m_iconType == InventoryIconType.Crafted && b.m_iconType == InventoryIconType.Inv)
            {
                swapType = SwapType.CraftedToInv;
            }

            return swapType;
        }
    }
}
