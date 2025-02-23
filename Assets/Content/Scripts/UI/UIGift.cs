using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Assets.Content.Scripts.UI
{
    public class UIGift : MonoBehaviour
    {
        [field: SerializeField] public Button Background { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Title { get; private set; }
        [field: SerializeField] public Image Icon { get; private set; }
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ButtonText { get; private set; }
        [field: SerializeField] public Image Back { get; private set; }
        [field: SerializeField] public Color ColorReady { get; private set; }
        [field: SerializeField] public Color ColorTaken { get; private set; }

        public delegate void OnGiftsDelegate(int index);
        public event OnGiftsDelegate OnGiftsSelected;

        private int _index;

        public void Init(GiftData giftData, int index)
        {
            _index = index;
            Title.SetText(giftData.Description);
            Icon.sprite = giftData.Icon;
        }

        public void Applay()
        {
            if (YandexGame.EnvironmentData.language == "ru")
            {
                ButtonText.SetText("Забрано");
            }
            else if (YandexGame.EnvironmentData.language == "en")
            {
                ButtonText.SetText("Taken");
            }
            else if (YandexGame.EnvironmentData.language == "tr")
            {
                ButtonText.SetText("Alınmış");
            }
            Background.interactable = false;
            OnGiftsSelected?.Invoke(_index);
            Back.color = ColorTaken;
        }
    }
}