using Assets.Content.Scripts.Statbuff;
using Assets.Content.Scripts.Unit;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Content.Scripts.UI.Weak
{
    public class UiWeakInventory : MonoBehaviour
    {
        [SerializeField] private MouseHoverScale mouseHoverScaleSelect;
        [SerializeField] private MouseHoverScale mouseHoverScaleDeSelect;
        [SerializeField] private UiWeakItem[] Items;
        [SerializeField] private UiWeakItem[] _dressImage;

        //buffs
        private float _healthBuff = 20f;

        private Dictionary<int, (UiWeakItem, UiWeakItem)> dressItems;

        private void Start()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i].OnSelect += mouseHoverScaleSelect.SetIndexSelect;
            }

            mouseHoverScaleSelect.OnSelected += id =>
            {
                if (id != -1)
                {
                    Dress(id, Items[id].Type);
                }
            };

            for (int i = 0; i < _dressImage.Length; i++)
            {
                _dressImage[i].OnSelect += mouseHoverScaleDeSelect.SetIndexSelect;
            }

            mouseHoverScaleDeSelect.OnSelected += id =>
            {
                if (id != -1)
                {
                    Undress(id, Items[id].Type);
                }
            };

            dressItems = new();
        }

        public void Dress(int id, InventoryWeakItemType type)
        {
            switch (type)
            {
                case InventoryWeakItemType.Health:
                    if (id < _dressImage.Length)
                    {
                        dressItems.Add(id, (Items[id], _dressImage[id]));
                        Items[id].gameObject.SetActive(false);
                        _dressImage[id].gameObject.SetActive(true);
                        UnitController.Instance.gameObject.AddComponent<HealthStatBuff>().SetValue(_healthBuff);
                    }
                    break;
            }
        }

        public void Undress(int id, InventoryWeakItemType type)
        {
            switch (type)
            {
                case InventoryWeakItemType.Health:
                    if (id < _dressImage.Length)
                    {
                        dressItems.Remove(id);
                        Items[id].gameObject.SetActive(true);
                        _dressImage[id].gameObject.SetActive(false);
                        UnitController.Instance.gameObject.TryGetComponent(out HealthStatBuff health);
                        Destroy(health);
                    }
                    break;
            }
        }
    }

    public enum InventoryWeakItemType
    {
        Health
    }
}