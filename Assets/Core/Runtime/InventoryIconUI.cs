using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MC.Core
{
    public enum SwapType
    {
        InvToInv = 0,
        InvToCraft = 1,
        CraftToCraft = 2,
        CraftToInv = 3
    }
    public class InventoryIconUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static System.Action<int, int, SwapType> OnSwapItem;

        //Slot ID 由InventorySystem/CraftSystem 设置
        private int m_slotID = 0;

        private bool m_isInventory = true;

        private Image m_icon;
        private Vector3 m_origin_pos = Vector3.zero;
        private Transform m_orign_parent;

        public void Init(int slotID, Image icon, bool isInventory)
        {
            m_slotID = slotID;
            m_icon = icon;

            m_origin_pos = m_icon.rectTransform.localPosition;
            m_orign_parent = m_icon.transform.parent;
            m_isInventory = isInventory;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_icon.raycastTarget = false;
            m_icon.transform.parent = m_icon.transform.GetComponentInParent<Canvas>().transform;
            m_icon.transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            m_icon.rectTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var rayHit = eventData.pointerCurrentRaycast;

            var targetIconUI = rayHit.gameObject?.transform?.parent?.GetComponent<InventoryIconUI>();

            if (targetIconUI != null)
            {
                var swapType = SwapType.InvToInv;

                if (m_isInventory && targetIconUI.m_isInventory)
                {
                    swapType = SwapType.InvToInv;
                }
                else if (m_isInventory && !targetIconUI.m_isInventory)
                {
                    swapType = SwapType.InvToCraft;
                }
                else if (!m_isInventory && targetIconUI.m_isInventory)
                {
                    swapType = SwapType.CraftToInv;
                }
                else if (!m_isInventory && !targetIconUI.m_isInventory)
                {
                    swapType = SwapType.CraftToCraft;
                }

                OnSwapItem?.Invoke(m_slotID, targetIconUI.GetSlotID(), swapType);

                Debug.Log($"Perform Swap {m_slotID} {targetIconUI.GetSlotID()} {swapType}");
            }

            m_icon.transform.parent = m_orign_parent;
            m_icon.rectTransform.localPosition = m_origin_pos;
            m_icon.raycastTarget = true;
        }

        public int GetSlotID()
        {
            return m_slotID;
        }


    }

}
