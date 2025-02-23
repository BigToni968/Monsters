using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Assets.Content.Scripts.UI
{
    [Serializable]
    public struct GiftData
    {
        public string Description;
        public Sprite Icon;
        public float Duration;
    }
    
    public class WindowGift : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private UIGift _uiGiftsPrefab;
        [SerializeField] private GiftData[] Gifts;
        [SerializeField] private Animator _animator;

        public int NoTake = 0;

        public delegate void GiftsGelegate();
        public event GiftsGelegate OnUpdate;

        private List<UIGift> _ui_GiftsSelected;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            _ui_GiftsSelected ??= new(Gifts.Length);
            for (int i = 0; i < Gifts.Length; i++)
            {
                UIGift temp = Instantiate(_uiGiftsPrefab, _content);
                temp.Init(Gifts[i], i);
                _ui_GiftsSelected.Add(temp);
                temp.ButtonText.text = $"выбрать";
                StartCoroutine(Timer(Gifts[i].Duration, temp.Background, temp.ButtonText, temp.Back, temp.ColorReady));
            }
        }

        public void Show()
        {
            _animator.SetTrigger("IsShow");

            for (int i = 0; i < _ui_GiftsSelected.Count; i++)
            {
                _ui_GiftsSelected[i].OnGiftsSelected += ApplayGifts;
                _ui_GiftsSelected[i].gameObject.SetActive(true);
            }
        }

        public void Hide()
        {
            _animator.SetTrigger("IsHide");

            for (int i = _ui_GiftsSelected.Count - 1; i >= 0; i--)
            {
                _ui_GiftsSelected[i].gameObject.SetActive(false);
                _ui_GiftsSelected[i].OnGiftsSelected -= ApplayGifts;
            }
        }

        public void ApplayGifts(int index)
        {
            NoTake--;
            switch (index)
            {
                case 0:
                    Debug.Log("Подарок 1");
                    break;
                case 1:
                    Debug.Log("Подарок 2");
                    break;
                case 2:
                    Debug.Log("Подарок 3");
                    break;
                case 3:
                    Debug.Log("Подарок 4");
                    break;
                case 4:
                    Debug.Log("Подарок 5");
                    break;
                case 5:
                    Debug.Log("Подарок 6");
                    break;
                case 6:
                    Debug.Log("Подарок 7");
                    break;
                case 7:
                    Debug.Log("Подарок 8");
                    break;
                case 8:
                    Debug.Log("Подарок 9");
                    break;
                case 9:
                    Debug.Log("Подарок 10");
                    break;
                case 10:
                    Debug.Log("Подарок 11");
                    break;
                case 11:
                    Debug.Log("Подарок 12");
                    break;
            }

            OnUpdate?.Invoke();
        }

        private IEnumerator Timer(float time, Button button, TextMeshProUGUI textButton, Image tempBack, Color tempColorReady)
        {
            while (time > 0)
            {
                time -= 1;
                textButton.SetText(Convert((int)time));
                yield return new WaitForSeconds(1f);
            }

            if (YandexGame.EnvironmentData.language == "ru")
            {
                textButton.SetText("Забрать");
            }
            else if (YandexGame.EnvironmentData.language == "en")
            {
                textButton.SetText("Take");
            }
            else if (YandexGame.EnvironmentData.language == "tr")
            {
                textButton.SetText("Al! al!");
            }

            button.interactable = true;
            tempBack.color = tempColorReady;
            NoTake++;
            OnUpdate?.Invoke();
        }

        public string Convert(int time)
        {
            int hours = (int)(time / 3600);
            int minutes = (int)((time % 3600) / 60);
            int seconds = (int)(time % 60);

            return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
        }
    }
}