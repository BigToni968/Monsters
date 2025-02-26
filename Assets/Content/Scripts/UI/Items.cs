using UnityEngine;
using System;

namespace Assets.Content.Scripts.UI
{
    public class Items : MonoBehaviour
    {
        [SerializeField] public TypeItems _typeItems;
        public float Time;
        public float Price;
        public float Value;
        public int Index;

        public event Action<Items> OnSelect;

        public void Selected()
        {
            OnSelect?.Invoke(this);
        }
    }
}