﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace MC.Core
{
    public class MobileTouchBar : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        private float m_XScale = 1, m_YScale = 1;

        private int currentTouchID = -1;

        private bool isDragging = false;

        private float clickTime;

        private const float clickTimeDeltaSlope = 0.2f;

        private Vector2 currentPointerPos;

        private Vector2 dragBeginPos;

        private void Start()
        {
            m_XScale = 160 / (float)Screen.width;
            m_YScale = 160 / (float)Screen.height;
        }

        private void Update()
        {
            if (isDragging)
            {
                ControlEvents.OnPressingScreen?.Invoke(currentPointerPos);
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerId == currentTouchID)
            {
                ControlEvents.OnCameraControllerInput?.Invoke(new InputData()
                {
                    x = eventData.delta.x * m_XScale,
                    y = eventData.delta.y * m_YScale
                });
            }

            currentPointerPos = eventData.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (currentTouchID == -1)
            {
                currentTouchID = eventData.pointerId;

                ControlEvents.OnCameraControllerInput?.Invoke(new InputData()
                {
                    x = 0,
                    y = 0
                });
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            currentTouchID = -1;

            ControlEvents.OnCameraControllerInput?.Invoke(new InputData()
            {
                x = 0,
                y = 0
            });
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            clickTime = Time.time;

            isDragging = true;

            currentPointerPos = eventData.position;
            dragBeginPos = eventData.position;

            ControlEvents.OnBeginPressScreen?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            var deltaTime = Time.time - clickTime;

            if (deltaTime < clickTimeDeltaSlope && Vector3.Magnitude(eventData.position - dragBeginPos) < Mathf.Epsilon)
            {
                ControlEvents.OnClickScreen?.Invoke(eventData.pressPosition);
            }

            isDragging = false;

            currentPointerPos = eventData.position;
            ControlEvents.OnEndPressScreen?.Invoke();
        }

    }

}

