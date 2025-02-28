using Assets.Content.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Content.Scripts.Others
{
    public class HoldHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private WindowMobileController _mobileController;
        [SerializeField] private float valueZoom;

        public void OnPointerDown(PointerEventData eventData)
        {
            _mobileController.SetZoom(valueZoom);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _mobileController.SetZoom(0);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _mobileController.SetZoom(0);
        }
    }
}