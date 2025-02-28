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
        [SerializeField] private int _levelMap1, _levelMap2, _levelMap3;
        [SerializeField] private Transform _stopBarrierMap1, _stopBarrierMap2, _stopBarrierMap3;
        [SerializeField] private Transform _canvas1, _canvas2, _canvas3;

        private void Start()
        {
            Load();
        }

        private void Load()
        {
            YandexGame.LoadProgress();
            if (MainUI.Instance.CurrentLevel >= _levelMap1 && YandexGame.savesData.IsBossDeathMap1)
            {
                _stopBarrierMap1.gameObject.SetActive(false);
                _canvas1.gameObject.SetActive(false);
            }

            if (MainUI.Instance.CurrentLevel >= _levelMap2 && YandexGame.savesData.IsBossDeathMap2)
            {
                _stopBarrierMap2.gameObject.SetActive(false);
                _canvas2.gameObject.SetActive(false);
            }

            if (MainUI.Instance.CurrentLevel >= _levelMap3 && YandexGame.savesData.IsBossDeathMap3)
            {
                _stopBarrierMap3.gameObject.SetActive(false);
                _canvas3.gameObject.SetActive(false);
            }
        }

        public void SetBarrier()
        {
            if (MainUI.Instance.CurrentLevel >= _levelMap1 && YandexGame.savesData.IsBossDeathMap1)
            {
                _stopBarrierMap1.gameObject.SetActive(false);
                _canvas1.gameObject.SetActive(false);
            }

            if (MainUI.Instance.CurrentLevel >= _levelMap2 && YandexGame.savesData.IsBossDeathMap2)
            {
                _stopBarrierMap2.gameObject.SetActive(false);
                _canvas2.gameObject.SetActive(false);
            }

            if (MainUI.Instance.CurrentLevel >= _levelMap3 && YandexGame.savesData.IsBossDeathMap3)
            {
                _stopBarrierMap3.gameObject.SetActive(false);
                _canvas3.gameObject.SetActive(false);
            }
        }
    }
}