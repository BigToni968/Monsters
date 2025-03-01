using Assets.Content.Scripts.Enemy;
using Assets.Content.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Assets.Content.Scripts.Others
{
    public class OpenPortal : MonoBehaviour
    {
        [SerializeField] private int _levelMap;
        [SerializeField] private Transform _stopBarrier;
        [SerializeField] private Transform _canvas;

        private void Start()
        {
            Load();
        }

        private void Load()
        {
            YandexGame.LoadProgress();
            if (MainUI.Instance.CurrentLevel >= _levelMap && YandexGame.savesData.IsBossDeathMap1 || YandexGame.savesData.IsBossDeathMap2 || YandexGame.savesData.IsBossDeathMap3)
            {
                _stopBarrier.gameObject.SetActive(false);
                _canvas.gameObject.SetActive(false);
            }
        }

        public void SetBarrier()
        {
            if (MainUI.Instance.CurrentLevel >= _levelMap && YandexGame.savesData.IsBossDeathMap1 || YandexGame.savesData.IsBossDeathMap2 || YandexGame.savesData.IsBossDeathMap3)
            {
                _stopBarrier.gameObject.SetActive(false);
                _canvas.gameObject.SetActive(false);
            }
        }
    }
}