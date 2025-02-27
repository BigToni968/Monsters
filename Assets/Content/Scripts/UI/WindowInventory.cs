using Assets.Content.Scripts.Others;
using Assets.Content.Scripts.Player;
using Assets.Content.Scripts.UI.Weak;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using static UnityEditor.Progress;

namespace Assets.Content.Scripts.UI
{
    public class WindowInventory : MonoBehaviour
    {
        [field: SerializeField] public List<UiWeakItem> ItemsEquipment { get; private set; } = new List<UiWeakItem>();

        [SerializeField] private Transform _contentEquipment;
        [SerializeField] private Transform _contentSprunki;
        [SerializeField] private UISprunki _prefabSprunki;
        [SerializeField] private List<UiWeakItem> weakItems;
        [SerializeField] private UiWeakInventory _weakInventory;

        private void Start()
        {
            Load();
            _weakInventory.Load();
        }

        private void Save(int index)
        {
            YandexGame.savesData.ItemsEquipment.Add(weakItems[index].IndexForSpawn);
            YandexGame.SaveProgress();
        }
        public void Load()
        {
            YandexGame.LoadProgress();
            for (int i = 0; i < YandexGame.savesData.ItemsEquipment.Count; i++)
            {
                LoadEquipment(weakItems[YandexGame.savesData.ItemsEquipment[i]]);
            }
        }

        public void AddItemEquipment(UiWeakItem item)
        {
            UiWeakItem _item = Instantiate(item, _contentEquipment);
            ItemsEquipment.Add(_item);
            Save(_item.IndexForSpawn);
        }


        private void LoadEquipment(UiWeakItem item)
        {
            UiWeakItem _item = Instantiate(item, _contentEquipment);
            ItemsEquipment.Add(_item);
        }

        public void ShowSprunki(Sprite sprite, int index, List<PlayerUnit> playerUnits, UpgradeManager upgradeManager, Animator animator, ParticleSystem particle)
        {
            UISprunki uISprunki = Instantiate(_prefabSprunki, _contentSprunki);
            uISprunki.Init(sprite, index, playerUnits, upgradeManager, animator, particle);
        }
    }
}