using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace Assets.Content.Scripts.UI
{
    public class MouseHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _scale;
        public Button Button;

        public int SelectId;
        public event Action<int> OnSelected;

        private Vector3 _originalScale;
        private Vector3 _hoverScale;

        private void Start()
        {
            _originalScale = transform.localScale;
            _hoverScale = new Vector3(_originalScale.x + _scale, _originalScale.y + _scale, _originalScale.z + _scale);

            if (Button == null)
            {
                return;
            }

            Button.onClick.AddListener(() =>
            {
                OnSelected?.Invoke(SelectId);
                SelectId = -1;
            });
        }

        public void SetIndexSelect(int index)
        {
            SelectId = index;
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