using Assets.Content.Scripts.Player;
using Assets.Content.Scripts.UI.Weak;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;
using static UnityEditor.Progress;

namespace Assets.Content.Scripts.UI
{
    public enum RewardType
    {
        None,
        Empty,
        Filled
    }

    [Serializable]
    public struct RewardData
    {
        public string LastTime;
        public DateTime Time;
        public int AllRewards;
        public int TakeCount;
        public int NotakeCount;
        public RewardType[] Rewards;

        public RewardData(string lastTime, DateTime date, int allRewards, int takeCount, int notakeCount, RewardType[] rewards)
        {
            LastTime = lastTime;
            Time = date;
            AllRewards = allRewards;
            TakeCount = takeCount;
            NotakeCount = notakeCount;
            Rewards = rewards;
        }
    }
    public class WindowDailyReward : MonoBehaviour
    {
        [SerializeField] private List<Button> _buttonDailyRewards;
        [SerializeField] private WindowInventory _windowInventory;
        [SerializeField] private WindowItem _windowItem;
        [SerializeField] private UiWeakItem[] _weakItems;
        [SerializeField] private Items[] _items;

        public delegate void RewardDelegate();
        public event RewardDelegate OnUpdateReward;

        public void OnEnable()
        {
            Init();

            for (int i = 0; i < _buttonDailyRewards.Count; i++)
            {
                _buttonDailyRewards[i].gameObject.SetActive(YandexGame.savesData.RewardData.Rewards[i] == RewardType.Filled);
                int index = i;
                _buttonDailyRewards[i].onClick.AddListener(() => TakeReward(index));
            }

            OnUpdateReward?.Invoke();
        }

        public void Init()
        {
            CheckDate();
            CheckRewards();
        }

        public void TakeReward(int index)
        {
            YandexGame.savesData.RewardData.Rewards[index] = RewardType.Empty;
            YandexGame.savesData.RewardData.NotakeCount--;
            YandexGame.savesData.RewardData.TakeCount++;
            _buttonDailyRewards[index].gameObject.SetActive(false);

            switch (index)
            {
                case 0:
                    _windowInventory.AddItemEquipment(_weakItems[0]);
                    break;
                case 1:
                    MainUI.Instance.AddMoney(5000);
                    break;
                case 2:
                    _windowItem.AddItem(_items[0]);
                    _windowItem.AddItem(_items[1]);
                    _windowItem.AddItem(_items[2]);
                    break;
                case 3:
                    for (int i = 0; i < 2; i++)
                    {
                        _windowInventory.AddItemEquipment(_weakItems[1]);
                    }
                    break;
                case 4:
                    MainUI.Instance.AddMoney(25000);
                    break;
                case 5:
                    for (int i = 0; i < 5; i++)
                    {
                        _windowItem.AddItem(_items[0]);
                        _windowItem.AddItem(_items[1]);
                        _windowItem.AddItem(_items[2]);
                    }
                    break;
                case 6:
                    MainUI.Instance.AddMoney(50000);
                    break;
            }

            if (ResetRewards())
            {
                CheckDate();
                return;
            }

            YandexGame.SaveProgress();
            OnUpdateReward?.Invoke();
        }

        public void OnDisable()
        {
            for (int i = 0; i < _buttonDailyRewards.Count; i++)
            {
                _buttonDailyRewards[i].gameObject.SetActive(false);
                _buttonDailyRewards[i].onClick.RemoveAllListeners();
            }
        }

        public void CheckDate()
        {
            YandexGame.LoadProgress();
            DateTime.TryParse(YandexGame.savesData.RewardData.LastTime, out YandexGame.savesData.RewardData.Time);

            if (ResetRewards())
            {
                CheckDate();
                return;
            }

            if (string.IsNullOrEmpty(YandexGame.savesData.RewardData.LastTime))
            {
                YandexGame.savesData.RewardData.Time = DateTime.Today.AddDays(-1);
                YandexGame.savesData.RewardData.LastTime = YandexGame.savesData.RewardData.Time.ToString();
                YandexGame.SaveProgress();
            }
        }

        public void CheckRewards()
        {
            for (int i = YandexGame.savesData.RewardData.Rewards.Length - 1; i >= 0; i--)
            {
                if (DateTime.Today.AddDays(-i) > YandexGame.savesData.RewardData.Time && YandexGame.savesData.RewardData.Rewards[i] == RewardType.None)
                {
                    YandexGame.savesData.RewardData.Rewards[i] = RewardType.Filled;
                    YandexGame.savesData.RewardData.NotakeCount++;
                }
            }

            YandexGame.SaveProgress();
            OnUpdateReward?.Invoke();
        }

        public bool ResetRewards()
        {
            if (YandexGame.savesData.RewardData.TakeCount == YandexGame.savesData.RewardData.AllRewards)
            {
                YandexGame.savesData.RewardData = new() { Rewards = new RewardType[YandexGame.savesData.RewardData.AllRewards], AllRewards = YandexGame.savesData.RewardData.AllRewards };
                YandexGame.SaveProgress();
                OnDisable();
                OnEnable();
                return true;
            }

            return false;
        }
    }
}