using Assets.Content.Scripts.Player;
using Assets.Content.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Assets.Content.Scripts.Others
{
    public class AdsManager : MonoBehaviour
    {
        [SerializeField] private int _delayIntro;
        [SerializeField] private WindowAds _windowAds;
        [SerializeField] private WindowItem _windowItem;
        [SerializeField] private WindowShop _windowShop;

        public int RewardID;

        private int _productID = -1;
        private float _timer;
        private float _timerPause = 2f;

        [field: SerializeField] public static AdsManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void Update()
        {
            Intro();
        }

        private void Start()
        {
            int interCD = 0;
            if (int.TryParse(YandexGame.GetFlag("InterCD"), out interCD))
            {
                YandexGame.Instance.infoYG.fullscreenAdInterval = interCD;
                _delayIntro = interCD;
            }
            else
            {
                YandexGame.Instance.infoYG.fullscreenAdInterval = 60;
                _delayIntro = 60;
            }
        }
        public void SetIDProduct(int id)
        {
            _productID = id;
        }
        public void ShowVideoReward(int index)
        {
            if (RewardID != -1)
            {
                RewardID = -1;
            }
            if (RewardID == -1)
            {
                RewardID = index;
                YandexGame.RewVideoShow(0);
            }
        }

        public void AddBuffExp()
        {
            if (RewardID == 0)
            {
                _windowItem.UseAds(0);
                RewardID = -1;
            }
        }

        public void AddBuffDamage()
        {
            if (RewardID == 1)
            {
                _windowItem.UseAds(1);
                RewardID = -1;
            }
        }
        public void BuyChest()
        {
            if (RewardID == 2)
            {
                _windowShop.BuyChestAds(_productID);
                RewardID = -1;
            }
        }
        public void BuyEquip()
        {
            if (RewardID == 3)
            {
                _windowShop.BuyEquipAds(_productID);
                RewardID = -1;
            }
        }
        public void BuyItem()
        {
            if (RewardID == 4)
            {
                _windowShop.BuyItemAds(_productID);
                RewardID = -1;
            }
        }

        private void Intro()
        {
            _timer += Time.deltaTime;

            if (_timer >= _delayIntro && !MainUI.Instance.IsCanvasEnable())
            {
                Time.timeScale = 0f;
                _windowAds.Show();
                _windowAds.SetTimer(_timerPause);
                _timerPause -= Time.unscaledDeltaTime;
                if (_timerPause <= 0)
                {
                    ShowIntroAds();
                    _timerPause = 2f;
                    _timer = 0f;
                    _windowAds.Hide();
                }
            }
        }
        private void ShowIntroAds()
        {
            YandexGame.FullscreenShow();
        }
    }
}