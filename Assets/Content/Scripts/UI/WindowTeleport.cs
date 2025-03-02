using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

namespace Assets.Content.Scripts.UI
{
    public class WindowTeleport : MonoBehaviour
    {
        [SerializeField] private Image _twoMap;
        [SerializeField] private Image _threeMap;
        [SerializeField] private Image _fourthMap;
        [SerializeField] private Button _twoMapButton;
        [SerializeField] private Button _threeMapButton;
        [SerializeField] private Button _fourthMapButton;

        private void Start()
        {
            CheckMapOpen();
        }

        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void CheckMapOpen()
        {
            YandexGame.LoadProgress();
            if (MainUI.Instance.CurrentLevel >= 7 && YandexGame.savesData.IsBossDeathMap1 )
            {
                _twoMap.gameObject.SetActive(false);
                _twoMapButton.interactable = true;
            }
            else
            {
                _twoMap.gameObject.SetActive(true);
                _twoMapButton.interactable = false;
            }

            if (MainUI.Instance.CurrentLevel >= 14 && YandexGame.savesData.IsBossDeathMap2)
            {
                _threeMap.gameObject.SetActive(false);
                _threeMapButton.interactable = true;
            }
            else
            {
                _threeMap.gameObject.SetActive(true);
                _threeMapButton.interactable = false;
            }

            if (MainUI.Instance.CurrentLevel >= 20 && YandexGame.savesData.IsBossDeathMap3)
            {
                _fourthMap.gameObject.SetActive(false);
                _fourthMapButton.interactable = true;
            }
            else
            {
                _fourthMap.gameObject.SetActive(true);
                _fourthMapButton.interactable = false;
            }
        }
    }
}