using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Assets.Content.Scripts.Others
{
    public class Tutorial : MonoUpdater
    {
        [SerializeField] private Transform _canvasRu;
        [SerializeField] private Transform _canvasEn;
        [SerializeField] private Transform _canvasTr;

        private void Start()
        {
            if (YandexGame.EnvironmentData.language == "ru")
            {
                _canvasRu.gameObject.SetActive(true);
            }
            else if (YandexGame.EnvironmentData.language == "en")
            {
                _canvasEn.gameObject.SetActive(true);
            }
            else if (YandexGame.EnvironmentData.language == "tr")
            {
                _canvasTr.gameObject.SetActive(true);
            }
        }

        public override void OnTick()
        {
            if (_canvasRu.gameObject.activeSelf)
            {
                _canvasRu.LookAt(Camera.main.transform);
            }
            else if (_canvasEn.gameObject.activeSelf)
            {
                _canvasEn.LookAt(Camera.main.transform);
            }
            else if (_canvasTr.gameObject.activeSelf)
            {
                _canvasTr.LookAt(Camera.main.transform);
            }
        }
    }
}