using UnityEngine;
using UnityEngine.EventSystems;

namespace MC.Core
{
    public class MobileMovementInput : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler
    {
        public float x, y;

        private bool isTouching = false;

        public void OnEndDrag(PointerEventData eventData)
        {
            isTouching = false;

            ControlEvents.OnControllerInput?.Invoke(new InputData()
            {
                x = 0,
                y = 0
            });
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isTouching = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isTouching = false;

            ControlEvents.OnControllerInput?.Invoke(new InputData()
            {
                x = 0,
                y = 0
            });
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isTouching = false;

            ControlEvents.OnControllerInput?.Invoke(new InputData()
            {
                x = 0,
                y = 0
            });
        }

        private void Update()
        {
            if (Util.IsMobile())
            {
                if (isTouching)
                {
                    ControlEvents.OnControllerInput?.Invoke(new InputData()
                    {
                        x = x,
                        y = y
                    });
                }

                if (Input.touchCount == 0)
                {
                    ControlEvents.OnControllerInput?.Invoke(new InputData()
                    {
                        x = 0,
                        y = 0
                    });
                }
            }
        }
    }
}
