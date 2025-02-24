using Assets.Content.Scripts.Others;
using Assets.Content.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Assets.Content.Scripts.UI
{
    public class UISprunki : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;

        private UpgradeManager _upgradeManager;
        public void Init(Sprite sprite, int index, List<PlayerUnit> playerUnits, UpgradeManager upgradeManager)
        {
            _image.sprite = sprite;
            _upgradeManager = upgradeManager;
            _button.onClick.AddListener(() => Activate(playerUnits, index));
        }
        private void Activate(List<PlayerUnit> playerUnits, int index)
        {
            _upgradeManager.Deactivate();
            playerUnits[index].Activate();
            YandexGame.savesData.CurrentIdPlayer = index;
            YandexGame.SaveProgress();
        }
    }
}