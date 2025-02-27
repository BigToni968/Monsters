using UnityEngine;
using System;
using UnityEngine.UI;

namespace Assets.Content.Scripts.UI
{
    public class Items : MonoBehaviour
    {
        [SerializeField] public TypeItems _typeItems;
        public float Time;
        public float Price;
        public float Value;
        public int Index;
        public Image Image;
        public Image ImageBuckGround;
        public Color ColorGreen;
        public Color ColorWhite;
        public event Action<Items> OnSelect;

        public void Selected()
        {
            ImageBuckGround.color = ColorGreen;
            OnSelect?.Invoke(this);
        }

        public void DeSelected()
        {
            ImageBuckGround.color = ColorWhite;
        }
    }
}