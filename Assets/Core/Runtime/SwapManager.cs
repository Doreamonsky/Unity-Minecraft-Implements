using System.Collections.Generic;
using UnityEngine;

namespace MC.Core
{
    public class SwapManager : MonoBehaviour
    {
        public static System.Action<int, int, SwapType> OnSwapItem;

        private List<InventoryIconUI> inventoryIcons = new List<InventoryIconUI>();

        public static SwapManager Instance;

        private InventoryIconUI prevIconUI;

        public void AppendIconUI(InventoryIconUI iconUI)
        {
            inventoryIcons.Add(iconUI);
        }

        private void Start()
        {
            Instance = this;

            InventoryIconUI.OnPointerClicked += (InventoryIconUI target) =>
            {
                //仅在创作中 需要交换物体等行为
                if (!Util.isCrafting)
                {
                    return;
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
                    var swapType = SwapType.InvToInv;

                    if (prevIconUI.m_iconType == InventoryIconType.Inv && target.m_iconType == InventoryIconType.Inv)
                    {
                        swapType = SwapType.InvToInv;
                    }
                    else if (prevIconUI.m_iconType == InventoryIconType.Inv && target.m_iconType == InventoryIconType.Craft)
                    {
                        swapType = SwapType.InvToCraft;
                    }
                    else if (prevIconUI.m_iconType == InventoryIconType.Craft && target.m_iconType == InventoryIconType.Inv)
                    {
                        swapType = SwapType.CraftToInv;
                    }
                    else if (prevIconUI.m_iconType == InventoryIconType.Craft && target.m_iconType == InventoryIconType.Craft)
                    {
                        swapType = SwapType.CraftToCraft;
                    }
                    else if (prevIconUI.m_iconType == InventoryIconType.Crafted && target.m_iconType == InventoryIconType.Inv)
                    {
                        swapType = SwapType.CraftedToInv;
                    }

                    OnSwapItem?.Invoke(prevIconUI.m_slotID, target.m_slotID, swapType);

                    Debug.Log($"Perform Swap {prevIconUI.m_slotID} {target.m_slotID} {swapType}");

                    prevIconUI = null;

                    foreach (var t in inventoryIcons)
                    {
                        t.ToggleSelectEffect(false);
                    }
                }
            };
        }
    }
}
