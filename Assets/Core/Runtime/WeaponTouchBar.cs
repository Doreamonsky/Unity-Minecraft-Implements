
using UnityEngine;
using UnityEngine.EventSystems;

namespace MC.Core
{
    public class WeaponTouchBar : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        private float m_XScale = 1, m_YScale = 1;

        private bool isPressing = false;

        private void Start()
        {
            m_XScale = 45 / (float)Screen.width;
            m_YScale = 45 / (float)Screen.height;
        }


        public void OnDrag(PointerEventData eventData)
        {
            ControlEvents.OnCameraControllerInput?.Invoke(new InputData()
            {
                x = eventData.delta.x * m_XScale,
                y = eventData.delta.y * m_YScale
            });
        }

        private void Update()
        {
            if (isPressing)
            {
                ControlEvents.OnGunFire?.Invoke();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isPressing = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPressing = false;
        }

    }

}
