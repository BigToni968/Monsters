using Assets.Content.Scripts.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

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
                    Debug.Log("1 день");
                    break;
                case 1:
                    Debug.Log("2 день");
                    break;
                case 2:
                    Debug.Log("3 день");
                    break;
                case 3:
                    Debug.Log("4 день");
                    break;
                case 4:
                    Debug.Log("5 день");
                    break;
                case 5:
                    Debug.Log("6 день");
                    break;
                case 6:
                    Debug.Log("7 день");
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