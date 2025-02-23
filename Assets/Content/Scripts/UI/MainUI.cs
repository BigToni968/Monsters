using Assets.Content.Scripts.Others;
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
        [SerializeField] private Slider _scoreSlider;
        [SerializeField] private Button _autoAttackButton;
        [SerializeField] private Sprite[] autoAttackSprites;
        [SerializeField] private WindowLuckySpin _windowLuckySpin;
        [SerializeField] private WindowDailyReward _windowDailyReward;
        [SerializeField] private WindowGift _windowGift;
        [SerializeField] private UINotification[] _uiNotifications;
        [SerializeField] private Transform _homePoint;
        [SerializeField] private Transform _unitController;


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
            ResetLevelScore(500);
            SetTextLevel();
            _windowLuckySpin.OnUpdate += UpdateSpin;
            _windowGift.OnUpdate += UpdateGifts;
            CheckDailyReward();
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
            }
        }

        public void AddScore(float score, float money)
        {
            Money += money;
            Money = Mathf.Clamp(Money, 0, float.MaxValue);
            Score += score;
            Score = Mathf.Clamp(Score, 0, _upgradeManager.LevelsData[_upgradeManager.LevelsData.Length - 1].Score);

            CurrentScore += score;
            _upgradeManager.Upgrade();
            SetTextLevel();
        }

        public void SetTextLevel()
        {
            float percent = CurrentScore == 0 ? 0 :((CurrentScore / NeedScore)) * 100;
            _scoreText.SetText($"{new IdleCurrency(CurrentScore).ToShortString()}/{new IdleCurrency(NeedScore).ToShortString()} ({Mathf.RoundToInt(percent)}%)");
            _currentLevelText.SetText($"Уровень {CurrentLevel}");
            _futureLevelText.SetText($"Уровень {NeedLevel}");
            _scoreSlider.value = CurrentScore;
            _moneyText.SetText($"{Money}");
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
                //AudioManager.Instance.Sound.PlayOneShot(AudioManager.Instance.NotificationActivated);
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
                //AudioManager.Instance.Sound.PlayOneShot(AudioManager.Instance.NotificationActivated);
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
    }
}