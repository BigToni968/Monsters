using Assets.Content.Scripts.TimeBuff;
using Assets.Content.Scripts.UI.Weak;
using Assets.Content.Scripts.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Assets.Content.Scripts.UI
{
    public class WindowItem : MonoBehaviour
    {
        [field: SerializeField] public List<Items> Items { get; private set; } = new List<Items>();

        [SerializeField] private Transform _contentItems;
        [SerializeField] private Items[] _items;

        private Items _item;

        private void Start()
        {
            Load();
        }

        private void Save(int index)
        {
            YandexGame.savesData.Items.Add(_items[index].Index);
            YandexGame.SaveProgress();
        }
        public void Load()
        {
            YandexGame.LoadProgress();
            for (int i = 0; i < YandexGame.savesData.Items.Count; i++)
            {
                LoadChest(_items[YandexGame.savesData.Items[i]]);
            }
        }

        private void LoadChest(Items item)
        {
            Items items = Instantiate(item, _contentItems);
            items.OnSelect += Selected;
            Items.Add(items);
        }

        public void AddItem(Items item)
        {
            Items items = Instantiate(item, _contentItems);
            Items.Add(items);
            Save(items.Index);
        }

        public void Selected(Items item)
        {
            _item = item;
        }

        public void Use()
        {
            if (_item == null)
                return;

            switch (_item._typeItems)
            {
                case TypeItems.Gold:
                    var buff = UnitController.Instance.gameObject.AddComponent<GoldTimeBuff>();
                    buff.SetValue(_item.Value);
                    buff.Duration = _item.Time;
                    MainUI.Instance.SliderGoldBuff.value = MainUI.Instance.SliderGoldBuff.maxValue = _item.Time;
                    buff.OnTime += x => MainUI.Instance.SliderGoldBuff.value = x;
                    buff.Removed += () =>
                    {
                        MainUI.Instance.SliderGoldBuff.value = MainUI.Instance.SliderGoldBuff.maxValue;
                        _item?.gameObject.SetActive(true);
                    };

                    buff.Play();
                    _item.gameObject.SetActive(false);
                    break;
            }
        }
    }
    public enum TypeItems
    {
        Exp,
        Damage,
        Gold,

    }
}