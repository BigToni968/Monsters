using Assets.Content.Scripts.Others;
using Assets.Content.Scripts.Statbuff;
using Assets.Content.Scripts.UI.Weak;
using Assets.Content.Scripts.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Assets.Content.Scripts.UI
{
    public class WindowChest : MonoBehaviour
    {
        public delegate void DelegateOpenChest();
        public event DelegateOpenChest IsOpen;
        [field: SerializeField] public List<UIChest> ItemsChest { get; private set; } = new List<UIChest>();
        [SerializeField] private Transform _contentChest;
        [SerializeField] private UIChest[] _chests;
        [SerializeField] private Animator _windowOpenChest;
        [SerializeField] private OpenChestEquip _openChestEquip;
        [SerializeField] private OpenChestItem _openChestItem;
        [SerializeField] private OpenChestGold _openChestGold;

        private TypeChest typeChest;

        private void Start()
        {
            Load();
        }

        private void Save(int index)
        {
            YandexGame.savesData.Chests.Add(_chests[index].Index);
            YandexGame.SaveProgress();
        }
        public void Load()
        {
            YandexGame.LoadProgress();
            for (int i = 0; i < YandexGame.savesData.Chests.Count; i++)
            {
                LoadChest(_chests[YandexGame.savesData.Chests[i]]);
            }
        }
        private void LoadChest(UIChest item)
        {
            UIChest uIChest = Instantiate(item, _contentChest);
            ItemsChest.Add(uIChest);
            switch (uIChest.TypeChest)
            {
                case TypeChest.Equip:
                    uIChest.Button.onClick.AddListener(() => OpenChestEquip(uIChest));
                    break;
                case TypeChest.Items:
                    uIChest.Button.onClick.AddListener(() => OpenChestItem(uIChest));
                    break;
                case TypeChest.Gold:
                    uIChest.Button.onClick.AddListener(() => OpenChestGold(uIChest));
                    break;
            }
        }
        public void AddChest(UIChest item)
        {
            UIChest uIChest = Instantiate(item, _contentChest);
            ItemsChest.Add(uIChest);
            switch (uIChest.TypeChest)
            {
                case TypeChest.Equip:
                    uIChest.Button.onClick.AddListener(() => OpenChestEquip(uIChest));
                    break;
                case TypeChest.Items:
                    uIChest.Button.onClick.AddListener(() => OpenChestItem(uIChest));
                    break;
                case TypeChest.Gold:
                    uIChest.Button.onClick.AddListener(() => OpenChestGold(uIChest));
                    break;
            }       
            Save(uIChest.Index);
        }
        public void RemoveChest(UIChest item)
        {
            ItemsChest.Remove(item);
            if (YandexGame.savesData.Chests.Contains(item.Index))
            {
                YandexGame.savesData.Chests.Remove(item.Index);
                YandexGame.SaveProgress();
            }
            Destroy(item.gameObject);
        }
        private void OpenChestEquip(UIChest uIChest)
        {
            ClosePanelChests();
            _openChestEquip.SetupItems(uIChest);
            _windowOpenChest.SetTrigger("IsShow");
            _openChestEquip.gameObject.SetActive(true);
        }
        private void OpenChestItem(UIChest uIChest)
        {
            ClosePanelChests();
            _openChestItem.SetupItems(uIChest);
            _windowOpenChest.SetTrigger("IsShow");
            _openChestItem.gameObject.SetActive(true);
        }
        private void OpenChestGold(UIChest uIChest)
        {
            ClosePanelChests();
            _openChestGold.SetupItems(uIChest);
            _windowOpenChest.SetTrigger("IsShow");
            _openChestGold.gameObject.SetActive(true);
        }

        private void ClosePanelChests()
        {
            _openChestEquip.gameObject.SetActive(false);
            _openChestItem.gameObject.SetActive(false);
            _openChestGold.gameObject.SetActive(false);
        }
    }
    public enum TypeChest
    {
        Equip,
        Items,
        Gold
    }
}