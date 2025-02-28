using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Content.Scripts.Others
{
    public class CameraControllerPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        public bool pressed = false;
        public int fingerId;
        public Vector2 LookInputVector;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject == gameObject)
            {
                pressed = true;
                fingerId = eventData.pointerId;
            }
            LookInputVector = Vector3.zero;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            pressed = false;
            LookInputVector = Vector3.zero;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            LookInputVector = new Vector3(eventData.delta.x, eventData.delta.y, 0);
        }
    }
}