using Assets.Content.Scripts.UI.Weak;
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
        [SerializeField] private WindowLuckySpin _windowLuckySpin;
        [SerializeField] private WindowItem _windowItem;
        [SerializeField] private Items[] _items;
        [SerializeField] private WindowInventory _windowInventory;
        [SerializeField] private UiWeakItem[] _weakItems;

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
                    _windowLuckySpin.UpdateSpin(1);
                    break;
                case 1:
                    MainUI.Instance.AddMoney(500);
                    break;
                case 2:
                    _windowItem.AddItem(_items[0]);
                    break;
                case 3:
                    MainUI.Instance.AddMoney(2500);
                    break;
                case 4:
                    for (int i = 0; i < 2; i++)
                    {
                        _windowInventory.AddItemEquipment(_weakItems[0]);
                    }
                    break;
                case 5:
                    for (int i = 0; i < 2; i++)
                    {
                        _windowInventory.AddItemEquipment(_weakItems[1]);
                    }
                    break;
                case 6:
                    MainUI.Instance.AddMoney(10000);
                    break;
                case 7:
                    for (int i = 0; i < 2; i++)
                    {
                        _windowItem.AddItem(_items[1]);
                    }
                    break;
                case 8:
                    for (int i = 0; i < 2; i++)
                    {
                        _windowItem.AddItem(_items[2]);
                    }
                    break;
                case 9:
                    for (int i = 0; i < 2; i++)
                    {
                        _windowInventory.AddItemEquipment(_weakItems[2]);
                    }
                    break;
                case 10:
                    for (int i = 0; i < 2; i++)
                    {
                        _windowInventory.AddItemEquipment(_weakItems[3]);
                    }
                    break;
                case 11:
                    MainUI.Instance.AddMoney(30000);
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