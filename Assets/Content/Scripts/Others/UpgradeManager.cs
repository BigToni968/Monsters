using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Content.Scripts.Player;
using Assets.Content.Scripts.UI;
using System.Reflection;
using Assets.Content.Scripts.Unit;
using YG;

namespace Assets.Content.Scripts.Others
{
    public class UpgradeManager : MonoBehaviour
    {
        [SerializeField] private List<PlayerUnit> _playerUnits;
        [SerializeField] private WindowInventory _windowInventory;

        public List<PlayerUnit> OpenUnit = new List<PlayerUnit>();
        public LevelData[] LevelsData;

        private void Start()
        {
            Load();
            LoadUnit();
            _playerUnits[MainUI.Instance.CurrentIdPlayer].Activate();
            Upgrade();
        }

        public void Upgrade()
        {
            if (MainUI.Instance.CurrentScore >= MainUI.Instance.NeedScore)
            {
                for (int i = 0; i < _playerUnits.Count; i++)
                {
                    _playerUnits[i].Deactivate();
                }
                if (MainUI.Instance.CurrentLevel == 0)
                {
                    MainUI.Instance.ResetLevelScore(LevelsData[1].Score);
                    _playerUnits[0].Activate();
                    _windowInventory.ShowSprunki(_playerUnits[0].Sprite, 0, _playerUnits, this);
                    YandexGame.savesData.CurrentIdPlayer = 0;
                    return;
                }
                MainUI.Instance.ResetLevelScore(LevelsData[MainUI.Instance.NeedLevel].Score);
                _playerUnits[MainUI.Instance.CurrentLevel - 1].Activate();
                YandexGame.savesData.CurrentIdPlayer = MainUI.Instance.CurrentLevel - 1;
                SetStatPlayer(_playerUnits[MainUI.Instance.CurrentLevel - 1].Model.Health, _playerUnits[MainUI.Instance.CurrentLevel - 1].Model.Damage);
                _windowInventory.ShowSprunki(_playerUnits[MainUI.Instance.CurrentLevel - 1].Sprite, MainUI.Instance.CurrentLevel - 1, _playerUnits, this);
                Save();
            }
        }

        private void SetStatPlayer(float health, float damage)
        {
            UnitController.Instance.MaxHealthPlayer += health;
            UnitController.Instance.CurrentHealthPlayer = UnitController.Instance.MaxHealthPlayer;
            UnitController.Instance.DamagePlayer += damage;
        }
        public void Deactivate()
        {
            for (int i = 0; i < _playerUnits.Count; i++)
            {
                _playerUnits[i].Deactivate();
            }
        }

        private void LoadUnit()
        {
            YandexGame.LoadProgress();
            if (YandexGame.savesData.OpenUnit <= 1) return;
            for (int i = 0; i < YandexGame.savesData.OpenUnit; i++)
            {
                _windowInventory.ShowSprunki(_playerUnits[i].Sprite, i, _playerUnits, this);
            }
        }
        private void Save()
        {
            YandexGame.savesData.OpenUnit += 1;
            YandexGame.savesData.MaxHealthPlayer = UnitController.Instance.MaxHealthPlayer;
            YandexGame.savesData.CurrentHealthPlayer = UnitController.Instance.CurrentHealthPlayer;
            YandexGame.savesData.DamagePlayer = UnitController.Instance.DamagePlayer;

            YandexGame.SaveProgress();
        }
        private void Load()
        {
            YandexGame.LoadProgress();
            UnitController.Instance.MaxHealthPlayer += YandexGame.savesData.MaxHealthPlayer;
            UnitController.Instance.CurrentHealthPlayer = YandexGame.savesData.CurrentHealthPlayer;
            UnitController.Instance.DamagePlayer += YandexGame.savesData.DamagePlayer;
        }
    }

    [Serializable]
    public struct LevelData
    {
        public float Level;
        public float Score;
    }
}