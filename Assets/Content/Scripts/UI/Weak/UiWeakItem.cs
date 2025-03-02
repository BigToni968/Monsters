using UnityEngine;
using System;
using UnityEngine.UI;

namespace Assets.Content.Scripts.UI.Weak
{
    public class UiWeakItem : MonoBehaviour
    {
        public int Index;
        public int IndexItems;
        public InventoryWeakItemType Type;
        public delegate void DelegateSelected(int index, InventoryWeakItemType type);
        public event DelegateSelected OnSelect;
        public bool IsBusy;
        public float Value;
        public float Price;
        public Image Image;
        public int IndexForSpawn;
        public bool IsDress;

        public void OnSelectButton()
        {
            OnSelect?.Invoke(Index, Type);
        }
    }
}