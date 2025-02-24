using UnityEngine;
using System;

namespace Assets.Content.Scripts.UI.Weak
{
    public class UiWeakItem : MonoBehaviour
    {
        public int Index;
        public InventoryWeakItemType Type;
        public event Action<int> OnSelect;

        public void OnSelectButton()
        {
            OnSelect?.Invoke(Index);
        }
    }
}