using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MC.Core
{
    public class InventoryIconUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static System.Action<int, int> OnSwapItem;

        //Slot ID 由InventorySystem 设置
        private int m_slotID = 0;

        private Image m_icon;
        private Vector3 m_origin_pos = Vector3.zero;
        private Transform m_orign_parent;

        public void Init(int slotID, Image icon)
        {
            m_slotID = slotID;
            m_icon = icon;

            m_origin_pos = m_icon.rectTransform.localPosition;
            m_orign_parent = m_icon.transform.parent;
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

            Debug.Log(rayHit.gameObject, rayHit.gameObject);

            var targetIconUI = rayHit.gameObject?.transform?.parent?.GetComponent<InventoryIconUI>();

            if (targetIconUI != null)
            {
                OnSwapItem?.Invoke(m_slotID, targetIconUI.GetSlotID());
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
