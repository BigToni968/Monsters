using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Content.Scripts.UI
{
    public class MouseHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _scale;

        private Vector3 _originalScale;
        private Vector3 _hoverScale;

        private void Start()
        {
            _originalScale = transform.localScale;
            _hoverScale = new Vector3(_originalScale.x + _scale, _originalScale.y + _scale, _originalScale.z + _scale);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = _hoverScale;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = _originalScale;
        }
    }
}