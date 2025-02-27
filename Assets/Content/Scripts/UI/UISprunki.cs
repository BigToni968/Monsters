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
        public void Init(Sprite sprite, int index, List<PlayerUnit> playerUnits, UpgradeManager upgradeManager, Animator animator, ParticleSystem particle)
        {
            _image.sprite = sprite;
            _upgradeManager = upgradeManager;
            _button.onClick.AddListener(() => Activate(playerUnits, index, animator, particle));
        }
        private void Activate(List<PlayerUnit> playerUnits, int index, Animator animator, ParticleSystem particle)
        {
            _upgradeManager.Deactivate();
            playerUnits[index].Activate();
            animator.SetTrigger("IsHide");
            particle.Play();
            YandexGame.savesData.CurrentIdPlayer = index;
            YandexGame.SaveProgress();
        }
    }
}