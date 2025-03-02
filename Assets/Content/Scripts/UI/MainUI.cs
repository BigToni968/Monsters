using Assets.Content.Scripts.Others;
using Assets.Content.Scripts.Unit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Assets.Content.Scripts.UI
{
    public class MainUI : MonoBehaviour
    {
        [field: SerializeField] public float Score { get; private set; }
        [field: SerializeField] public float Money { get; private set; }
        [field: SerializeField] public float NeedScore { get; private set; }
        [field: SerializeField] public float CurrentScore { get; private set; }
        [field: SerializeField] public int CurrentLevel { get; private set; }
        [field: SerializeField] public int NeedLevel { get; private set; }
        [field: SerializeField] public bool AutoAttack { get; private set; }

        [SerializeField] private UpgradeManager _upgradeManager;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _currentLevelText;
        [SerializeField] private TextMeshProUGUI _futureLevelText;
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private TextMeshProUGUI _healthMaxStat;
        [SerializeField] private TextMeshProUGUI _damageMaxStat;
        [SerializeField] private Slider _scoreSlider;
        [SerializeField] private Button _autoAttackButton;
        [SerializeField] private Sprite[] autoAttackSprites;
        [SerializeField] private WindowLuckySpin _windowLuckySpin;
        [SerializeField] private WindowDailyReward _windowDailyReward;
        [SerializeField] private WindowInventory _windowInventory;
        [SerializeField] private WindowGift _windowGift;
        [SerializeField] private WindowTeleport _windowTeleport;
        [SerializeField] private UINotification[] _uiNotifications;
        [SerializeField] private Transform _homePoint;
        [SerializeField] private Transform _unitController;
        [SerializeField] private Canvas[] _allCanvas;

        public OpenPortal OpenPortal;
        public Slider SliderGoldBuff;
        public Slider SliderExpBuff;
        public Slider SliderDamageBuff;
        public Image GoldBuffImage;
        public Image ExpBuffImage;
        public Image DamageBuffImage;

        public int CurrentIdPlayer;
        public float BuffExp = 1;
        public float BuffGold = 1;
        public float BuyBuffGold = 1;
        public float BuyBuffExp = 1;

        [SerializeField] public static MainUI Instance { get; private set; }


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
            Load();
        }

        private void Update()
        {
            if (!_windowLuckySpin.IsTurn && _windowLuckySpin.Coroutine == null && _windowLuckySpin.TotalSpin <= 0)
            {
                _windowLuckySpin.Coroutine = StartCoroutine(_windowLuckySpin.Timer(_windowLuckySpin.Time, _windowLuckySpin._timerText));
            }
            if (_windowLuckySpin.IsTurn)
            {
                _windowLuckySpin._timerText.text = "";
            }
        }

        private void Start()
        {
            SetTextLevel();
            ChangeMoney(0);
            _windowLuckySpin.OnUpdate += UpdateSpin;
            _windowGift.OnUpdate += UpdateGifts;
            CheckDailyReward();
        }

        private void Load()
        {
            YandexGame.LoadProgress();
            Money = YandexGame.savesData.Money;
            Score = YandexGame.savesData.Score;
            NeedLevel = YandexGame.savesData.NeedLevel;
            CurrentLevel = YandexGame.savesData.CurrentLevel;
            NeedScore = YandexGame.savesData.NeedScore;
            CurrentScore = YandexGame.savesData.CurrentScore;
            CurrentIdPlayer = YandexGame.savesData.CurrentIdPlayer;
        }

        private void Save()
        {
            YandexGame.savesData.Money = Money;
            YandexGame.savesData.Score = Score;
            YandexGame.savesData.NeedLevel = NeedLevel;
            YandexGame.savesData.CurrentLevel = CurrentLevel;
            YandexGame.savesData.CurrentScore = CurrentScore;
            YandexGame.savesData.NeedScore = NeedScore;
            YandexGame.SaveProgress();
        }

        public void ResetLevelScore(float needScore)
        {
            if (CurrentScore == _upgradeManager.LevelsData[_upgradeManager.LevelsData.Length - 2].Level)
            {
                CurrentLevel++;
            }
            else
            {
                CurrentScore = 0;
                NeedScore = needScore;
                CurrentLevel++;
                NeedLevel++;
                _scoreSlider.maxValue = needScore;
                _scoreSlider.minValue = 0;
                SetTextLevel();
            }
        }

        public void AddScore(float score, float money)
        {
            ChangeMoney(money);
            Score += (score * BuffExp * BuyBuffExp);
            Score = Mathf.Clamp(Score, 0, _upgradeManager.LevelsData[_upgradeManager.LevelsData.Length - 1].Score);

            CurrentScore += (score * BuffExp * BuyBuffExp);
            _upgradeManager.Upgrade();
            SetTextLevel();
            Save();
            if (OpenPortal != null)
            {
                OpenPortal.SetBarrier();
            }
            _windowTeleport.CheckMapOpen();
        }

        public void SetTextLevel()
        {
            float percent = CurrentScore == 0 ? 0 : ((CurrentScore / NeedScore)) * 100;
            _scoreText.SetText($"{new IdleCurrency(CurrentScore).ToShortString()}/{new IdleCurrency(NeedScore).ToShortString()} ({Mathf.RoundToInt(percent)}%)");
            _currentLevelText.SetText($"Уровень {CurrentLevel}");
            _futureLevelText.SetText($"Уровень {NeedLevel}");
            _scoreSlider.maxValue = NeedScore;
            _scoreSlider.minValue = 0;
            _scoreSlider.value = CurrentScore;
        }

        public void SetAutoAttack()
        {
            AutoAttack = !AutoAttack;
            _autoAttackButton.image.sprite = AutoAttack ? autoAttackSprites[1] : autoAttackSprites[0];
        }

        public void ReturnToSpawn()
        {
            _unitController.position = _homePoint.position;
        }

        public void UpdateSpin()
        {
            if (_windowLuckySpin.TotalSpin > 0)
            {
                _uiNotifications[0].Set(_windowLuckySpin.TotalSpin.ToString());
                _uiNotifications[0].Show();
                AudioManager.Instance.Sound.PlayOneShot(AudioManager.Instance.NotificationClip);
                return;
            }

            _uiNotifications[0].Hide();
        }

        private void UpdateDailyReward()
        {
            if (YandexGame.savesData.RewardData.NotakeCount > 0)
            {
                _uiNotifications[1].Set(YandexGame.savesData.RewardData.NotakeCount.ToString());
                _uiNotifications[1].Show();
                return;
            }

            _uiNotifications[1].Hide();
        }

        private void UpdateGifts()
        {
            if (_windowGift.NoTake > 0)
            {
                _uiNotifications[2].Set(_windowGift.NoTake.ToString());
                _uiNotifications[2].Show();
                AudioManager.Instance.Sound.PlayOneShot(AudioManager.Instance.NotificationClip);
                return;
            }

            _uiNotifications[2].Hide();
        }

        private void CheckDailyReward()
        {
            _windowDailyReward.Init();
            _windowDailyReward.OnUpdateReward += UpdateDailyReward;
            UpdateDailyReward();
        }

        private void OnDestroy()
        {
            _windowGift.OnUpdate -= UpdateGifts;
            _windowDailyReward.OnUpdateReward -= UpdateDailyReward;
            _windowLuckySpin.OnUpdate -= UpdateSpin;
        }

        public bool IsCanvasEnable()
        {
            return _allCanvas[0].enabled || _allCanvas[1].enabled || _allCanvas[2].enabled || _allCanvas[3].enabled || _allCanvas[4].enabled || _allCanvas[5].enabled || _allCanvas[6].enabled || _allCanvas[7].enabled;
        }

        public void ChangeMoney(float money)
        {
            if (money < 0)
            {
                Money += money;
            }
            else
            {
                Money += (money * BuffGold * BuyBuffGold);
            }

            Money = Mathf.Clamp(Money, 0, float.MaxValue);
            _moneyText.SetText($"{new IdleCurrency(Money).ToShortString()}");
            Save();
        }
        public void AddMoney(float money)
        {
            Money += money;
            Money = Mathf.Clamp(Money, 0, float.MaxValue);
            _moneyText.SetText($"{new IdleCurrency(Money).ToShortString()}");
            Save();
        }
        public void SetStat()
        {
            _healthMaxStat.SetText($"{new IdleCurrency(UnitController.Instance.MaxHealthPlayer + UnitController.Instance.HealthPlayerBuff).ToShortString()}");
            _damageMaxStat.SetText($"{new IdleCurrency(UnitController.Instance.DamagePlayerStatic + UnitController.Instance.DamagePlayerBuff).ToShortString()}");
        }
    }
}