using Assets.Content.Scripts.UI.Weak;
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
            Items.Add(items);
        }
        public void AddItem(Items item)
        {
            Items items = Instantiate(item, _contentItems);
            Items.Add(items);
            Save(items.Index);
        }
    }
    public enum TypeItems
    {
        X2Exp2,
        X2Exp5,
        X2Damage2,
        X2Damage5,
        X2Gold2,
        X2Gold5,
    }
}