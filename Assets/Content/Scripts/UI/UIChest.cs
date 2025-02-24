using Assets.Content.Scripts.UI.Weak;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Content.Scripts.UI
{
    public class UIChest : MonoBehaviour
    {
        [SerializeField] private TypeChest typeChest;
        [SerializeField] private List<Items> _items;
        [SerializeField] private List<UiWeakItem> _uiWeakItem;
        [SerializeField] private List<UIGold> _uIGolds;

        public int Index;
        public float Price;
    }
}