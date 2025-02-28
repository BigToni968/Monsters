using Assets.Content.Scripts.UI;
using Assets.Content.Scripts.Unit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using YG;

public class WindowParameters : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textDamage;
    [SerializeField] private TextMeshProUGUI _textHealth;
    [SerializeField] private TextMeshProUGUI _textExp;
    [SerializeField] private TextMeshProUGUI _textGold;
    [SerializeField] private TextMeshProUGUI _textScale;
    [SerializeField] private int _countUpgradeDamage;
    [SerializeField] private int _countUpgradeHealth;
    [SerializeField] private int _countUpgradeExp;
    [SerializeField] private int _countUpgradeGold;
    
    [SerializeField] private int _maxUpgrade;
    [SerializeField] private float _value;
    [SerializeField] private float _valuePercent;
    [SerializeField] private float _price;
    [SerializeField] private float _scaleAmount = 1.2f;
    [SerializeField] private float _scaleDuration = 0.7f;

    private Vector3 originalScale;
    private Coroutine _coroutine;

    private void Start()
    {
        originalScale = _textScale.transform.localScale;
        Load();
        SetText();
    }
    private void Load()
    {
        YandexGame.LoadProgress();

        _countUpgradeDamage = YandexGame.savesData.TotalUpgradeDamage;
        _countUpgradeHealth = YandexGame.savesData.TotalUpgradeHealth;
        _countUpgradeExp = YandexGame.savesData.TotalUpgradeExp;
        _countUpgradeGold = YandexGame.savesData.TotalUpgradeGold;
        MainUI.Instance.BuyBuffExp = YandexGame.savesData.BuyBuffExp;
        MainUI.Instance.BuyBuffGold = YandexGame.savesData.BuyBuffGold;
    }
    private void Save()
    {
        YandexGame.savesData.TotalUpgradeExp = _countUpgradeExp;
        YandexGame.savesData.TotalUpgradeGold = _countUpgradeGold;
        YandexGame.savesData.BuyBuffExp = MainUI.Instance.BuyBuffExp;
        YandexGame.savesData.BuyBuffGold = MainUI.Instance.BuyBuffGold;

        YandexGame.SaveProgress();
    }

    public void UpgradeDamage()
    {
        if (_countUpgradeDamage < _maxUpgrade)
        {
            if (MainUI.Instance.Money >= _price)
            {
                MainUI.Instance.ChangeMoney(-_price);
                _countUpgradeDamage++;
                SetText();
                UnitController.Instance.DamagePlayerStatic += _value;
                MainUI.Instance.SetStat();
                YandexGame.savesData.TotalUpgradeDamage = _countUpgradeDamage;
                UnitController.Instance.Save();
            }
            else
            {
                if (_coroutine == null)
                {
                    if (YandexGame.EnvironmentData.language == "ru")
                    {
                        _textScale.SetText("Недостаточно монет!");
                    }
                    else if (YandexGame.EnvironmentData.language == "en")
                    {
                        _textScale.SetText("Not enough coins!");
                    }
                    else if (YandexGame.EnvironmentData.language == "tr")
                    {
                        _textScale.SetText("Yeterli para yok!");
                    }
                    _coroutine = StartCoroutine(ScaleAndDeactivate());
                }
            }
        }
        else
        {
            if (_coroutine == null)
            {
                if (YandexGame.EnvironmentData.language == "ru")
                {
                    _textScale.SetText("Максимум прокачки!");
                }
                else if (YandexGame.EnvironmentData.language == "en")
                {
                    _textScale.SetText("Maximum leveling!");
                }
                else if (YandexGame.EnvironmentData.language == "tr")
                {
                    _textScale.SetText("Maksimum pompalama!");
                }
                _coroutine = StartCoroutine(ScaleAndDeactivate());
            }
        }
    }

    public void UpgradeHealth()
    {
        if (_countUpgradeHealth < _maxUpgrade)
        {
            if (MainUI.Instance.Money >= _price)
            {
                MainUI.Instance.ChangeMoney(-_price);
                _countUpgradeHealth++;
                SetText();
                UnitController.Instance.MaxHealthPlayer += _value;
                UnitController.Instance.CurrentHealthPlayer += _value;
                MainUI.Instance.SetStat();
                YandexGame.savesData.TotalUpgradeHealth = _countUpgradeHealth;
                UnitController.Instance.Save();
            }
            else
            {
                if (_coroutine == null)
                {
                    if (YandexGame.EnvironmentData.language == "ru")
                    {
                        _textScale.SetText("Недостаточно монет!");
                    }
                    else if (YandexGame.EnvironmentData.language == "en")
                    {
                        _textScale.SetText("Not enough coins!");
                    }
                    else if (YandexGame.EnvironmentData.language == "tr")
                    {
                        _textScale.SetText("Yeterli para yok!");
                    }
                    
                    _coroutine = StartCoroutine(ScaleAndDeactivate());
                }
            }
        }
        else
        {
            if (_coroutine == null)
            {
                if (YandexGame.EnvironmentData.language == "ru")
                {
                    _textScale.SetText("Максимум прокачки!");
                }
                else if (YandexGame.EnvironmentData.language == "en")
                {
                    _textScale.SetText("Maximum leveling!");
                }
                else if (YandexGame.EnvironmentData.language == "tr")
                {
                    _textScale.SetText("Maksimum pompalama!");
                }
                _coroutine = StartCoroutine(ScaleAndDeactivate());
            }
        }
    }

    public void UpgradeExp()
    {
        if (_countUpgradeExp < _maxUpgrade)
        {
            if (MainUI.Instance.Money >= _price)
            {
                MainUI.Instance.ChangeMoney(-_price);
                _countUpgradeExp++;
                SetText();
                MainUI.Instance.BuyBuffExp += _valuePercent;
                Save();
            }
            else
            {
                if (_coroutine == null)
                {
                    if (YandexGame.EnvironmentData.language == "ru")
                    {
                        _textScale.SetText("Недостаточно монет!");
                    }
                    else if (YandexGame.EnvironmentData.language == "en")
                    {
                        _textScale.SetText("Not enough coins!");
                    }
                    else if (YandexGame.EnvironmentData.language == "tr")
                    {
                        _textScale.SetText("Yeterli para yok!");
                    }
                    _coroutine = StartCoroutine(ScaleAndDeactivate());
                }
            }
        }
        else
        {
            if (_coroutine == null)
            {
                if (YandexGame.EnvironmentData.language == "ru")
                {
                    _textScale.SetText("Максимум прокачки!");
                }
                else if (YandexGame.EnvironmentData.language == "en")
                {
                    _textScale.SetText("Maximum leveling!");
                }
                else if (YandexGame.EnvironmentData.language == "tr")
                {
                    _textScale.SetText("Maksimum pompalama!");
                }
                _coroutine = StartCoroutine(ScaleAndDeactivate());
            }
        }
    }
    
    public void UpgradeGold()
    {
        if (_countUpgradeGold < _maxUpgrade)
        {
            if (MainUI.Instance.Money >= _price)
            {
                MainUI.Instance.ChangeMoney(-_price);
                _countUpgradeGold++;
                SetText();
                MainUI.Instance.BuyBuffGold += _valuePercent;
                Save();
            }
            else
            {
                if (_coroutine == null)
                {
                    if (YandexGame.EnvironmentData.language == "ru")
                    {
                        _textScale.SetText("Недостаточно монет!");
                    }
                    else if (YandexGame.EnvironmentData.language == "en")
                    {
                        _textScale.SetText("Not enough coins!");
                    }
                    else if (YandexGame.EnvironmentData.language == "tr")
                    {
                        _textScale.SetText("Yeterli para yok!");
                    }
                    _coroutine = StartCoroutine(ScaleAndDeactivate());
                }
            }
        }
        else
        {
            if (_coroutine == null)
            {
                if (YandexGame.EnvironmentData.language == "ru")
                {
                    _textScale.SetText("Максимум прокачки!");
                }
                else if (YandexGame.EnvironmentData.language == "en")
                {
                    _textScale.SetText("Maximum leveling!");
                }
                else if (YandexGame.EnvironmentData.language == "tr")
                {
                    _textScale.SetText("Maksimum pompalama!");
                }
                _coroutine = StartCoroutine(ScaleAndDeactivate());
            }
        }
    }
    private void SetText()
    {
        _textExp.SetText($"{_countUpgradeExp}/{_maxUpgrade}");
        _textHealth.SetText($"{_countUpgradeHealth}/{_maxUpgrade}");
        _textDamage.SetText($"{_countUpgradeDamage}/{_maxUpgrade}");
        _textGold.SetText($"{_countUpgradeGold}/{_maxUpgrade}");
    }
    private IEnumerator ScaleAndDeactivate()
    {
        _textScale.gameObject.SetActive(true);

        Vector3 targetScale = originalScale * _scaleAmount;
        yield return StartCoroutine(ScaleTo(targetScale, _scaleDuration));

        _textScale.gameObject.SetActive(false);

        _textScale.transform.localScale = originalScale;
        _coroutine = null;
    }

    private IEnumerator ScaleTo(Vector3 target, float duration)
    {
        Vector3 initialScale = _textScale.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            _textScale.transform.localScale = Vector3.Lerp(initialScale, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _textScale.transform.localScale = target;
    }
}
