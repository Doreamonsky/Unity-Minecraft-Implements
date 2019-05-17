using UnityEngine;
using UnityEngine.EventSystems;

namespace MC.Core
{
    public class MobileTouchBar : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        private float m_XScale = 1, m_YScale = 1;

        private int currentTouchID = -1;

        private void Start()
        {
            m_XScale = 6000 / (float)Screen.width;
            m_YScale = 6000 / (float)Screen.height;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerId == currentTouchID)
            {
                CameraController.OnControllerInput?.Invoke(new InputData()
                {
                    x = eventData.delta.x * m_XScale,
                    y = eventData.delta.y * m_YScale
                });
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (currentTouchID == -1)
            {
                currentTouchID = eventData.pointerId;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            currentTouchID = -1;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ControlEvents.OnClickScreen?.Invoke(eventData.pressPosition);
        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }
    }

}
