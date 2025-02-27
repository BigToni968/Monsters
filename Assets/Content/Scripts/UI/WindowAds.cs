using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

namespace Assets.Content.Scripts.UI
{
    public class WindowAds : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TextMeshProUGUI _timerText;


        public void Show()
        {
            _canvas.enabled = true;
            _timerText.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _canvas.enabled = false;
            _timerText.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }

        public void SetTimer(float timer)
        {
            if (YandexGame.EnvironmentData.language == "ru")
            {
                _timerText.SetText($"Реклама: {MathF.Round(timer)}");
            }
            else if (YandexGame.EnvironmentData.language == "en")
            {
                _timerText.SetText($"Advertisement: {MathF.Round(timer)}");
            }
            else if (YandexGame.EnvironmentData.language == "tr")
            {
                _timerText.SetText($"Reklam: {MathF.Round(timer)}");
            }
        }
    }
}