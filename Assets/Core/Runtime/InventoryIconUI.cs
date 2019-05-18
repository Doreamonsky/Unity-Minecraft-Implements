using System.Collections;
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
        CraftToInv = 3,
        CraftedToInv = 4 //生成物体
    }

    public enum InventoryIconType
    {
        Inv = 0, Craft = 1, Crafted = 2
    }
    public class InventoryIconUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public static System.Action<InventoryIconUI> OnPointerClicked, OnPointerEntered, OnPointerExited;

        //Slot ID 由InventorySystem/CraftSystem 设置
        public int m_slotID = 0;

        public InventoryIconType m_iconType;

        public Image m_icon;

        public GameObject m_selectedEffect;

        public void Init(int slotID, GameObject selectedEffect, Image icon, InventoryIconType iconType)
        {
            m_slotID = slotID;
            m_selectedEffect = selectedEffect;
            m_icon = icon;
            m_iconType = iconType;

            SwapManager.Instance.AppendIconUI(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnPointerClicked?.Invoke(this);

            StopCoroutine(PointerEnterNotClick());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            StartCoroutine(PointerEnterNotClick());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExited?.Invoke(this);
        }

        private IEnumerator PointerEnterNotClick()
        {
            yield return new WaitForSeconds(0.2f);

            OnPointerEntered?.Invoke(this);
        }
        public void ToggleSelectEffect(bool state)
        {
            m_selectedEffect.SetActive(state);
        }
        //private int m_childIndex = 0;
        //private Vector3 m_origin_pos = Vector3.zero;
        //private Transform m_orign_parent;

        //    public void Init(int slotID, Image icon, InventoryIconType iconType)
        //    {
        //        m_slotID = slotID;
        //        m_icon = icon;

        //        m_childIndex = m_icon.transform.GetSiblingIndex();
        //        m_origin_pos = m_icon.rectTransform.localPosition;
        //        m_orign_parent = m_icon.transform.parent;
        //        m_iconType = iconType;
        //    }

        //    public void OnBeginDrag(PointerEventData eventData)
        //    {
        //        m_icon.raycastTarget = false;
        //        m_icon.transform.parent = m_icon.transform.GetComponentInParent<Canvas>().transform;
        //        m_icon.transform.SetAsLastSibling();
        //    }

        //    public void OnDrag(PointerEventData eventData)
        //    {
        //        m_icon.rectTransform.position = eventData.position;
        //    }

        //    public void OnEndDrag(PointerEventData eventData)
        //    {
        //        var rayHit = eventData.pointerCurrentRaycast;

        //        var targetIconUI = rayHit.gameObject?.transform?.parent?.GetComponent<InventoryIconUI>();

        //        if (targetIconUI != null)
        //        {

        //            var swapType = SwapType.InvToInv;

        //            if (m_iconType == InventoryIconType.Inv && targetIconUI.m_iconType == InventoryIconType.Inv)
        //            {
        //                swapType = SwapType.InvToInv;
        //            }
        //            else if (m_iconType == InventoryIconType.Inv && targetIconUI.m_iconType == InventoryIconType.Craft)
        //            {
        //                swapType = SwapType.InvToCraft;
        //            }
        //            else if (m_iconType == InventoryIconType.Craft && targetIconUI.m_iconType == InventoryIconType.Inv)
        //            {
        //                swapType = SwapType.CraftToInv;
        //            }
        //            else if (m_iconType == InventoryIconType.Craft && targetIconUI.m_iconType == InventoryIconType.Craft)
        //            {
        //                swapType = SwapType.CraftToCraft;
        //            }
        //            else if (m_iconType == InventoryIconType.Crafted && targetIconUI.m_iconType == InventoryIconType.Inv)
        //            {
        //                swapType = SwapType.CraftedToInv;
        //            }

        //            OnSwapItem?.Invoke(m_slotID, targetIconUI.GetSlotID(), swapType);

        //            Debug.Log($"Perform Swap {m_slotID} {targetIconUI.GetSlotID()} {swapType}");

        //        }

        //        m_icon.transform.parent = m_orign_parent;

        //        m_icon.transform.SetSiblingIndex(m_childIndex);
        //        m_icon.rectTransform.localPosition = m_origin_pos;
        //        m_icon.raycastTarget = true;
        //    }

        //    public int GetSlotID()
        //    {
        //        return m_slotID;
        //    }

        //    public void OnPointerClick(PointerEventData eventData)
        //    {
        //        if (previousClickIcon == null)
        //        {
        //            previousClickIcon = this;
        //        }
        //        else
        //        {
        //            var targetIconUI = this;

        //            var swapType = SwapType.InvToInv;

        //            if (previousClickIcon.m_iconType == InventoryIconType.Inv && targetIconUI.m_iconType == InventoryIconType.Inv)
        //            {
        //                swapType = SwapType.InvToInv;
        //            }
        //            else if (previousClickIcon.m_iconType == InventoryIconType.Inv && targetIconUI.m_iconType == InventoryIconType.Craft)
        //            {
        //                swapType = SwapType.InvToCraft;
        //            }
        //            else if (previousClickIcon.m_iconType == InventoryIconType.Craft && targetIconUI.m_iconType == InventoryIconType.Inv)
        //            {
        //                swapType = SwapType.CraftToInv;
        //            }
        //            else if (previousClickIcon.m_iconType == InventoryIconType.Craft && targetIconUI.m_iconType == InventoryIconType.Craft)
        //            {
        //                swapType = SwapType.CraftToCraft;
        //            }
        //            else if (previousClickIcon.m_iconType == InventoryIconType.Crafted && targetIconUI.m_iconType == InventoryIconType.Inv)
        //            {
        //                swapType = SwapType.CraftedToInv;
        //            }

        //            OnSwapItem?.Invoke(previousClickIcon.m_slotID, targetIconUI.m_slotID, swapType);

        //            previousClickIcon = null;
        //        }
        //    }
        //}

    }
}
