using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Assets.Content.Scripts.UI
{
    public class WindowChest : MonoBehaviour
    {
        [field: SerializeField] public List<UIChest> ItemsChest { get; private set; } = new List<UIChest>();
        [SerializeField] private Transform _contentChest;
        [SerializeField] private UIChest[] _chests;

        private void Start()
        {
            Load();
        }

        private void Save(int index)
        {
            YandexGame.savesData.Chests.Add(ItemsChest[index].Index);
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
        }
        public void AddChest(UIChest item)
        {
            UIChest uIChest = Instantiate(item, _contentChest);
            ItemsChest.Add(uIChest);
            Save(uIChest.Index);
        }
    }
    public enum TypeChest
    {
        Equip,
        Items,
        Gold
    }
}