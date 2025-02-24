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
        private void Start()
        {
            Load();
        }

        private void Save()
        {
            YandexGame.savesData.Items = Items;
            YandexGame.SaveProgress();
        }
        public void Load()
        {
            YandexGame.LoadProgress();
            Items = YandexGame.savesData.Items;
        }

        public void AddItem(Items item)
        {
            Items items = Instantiate(item, _contentItems);
            Items.Add(items);
            Save();
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