using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Content.Scripts.Player;
using Assets.Content.Scripts.UI;
using System.Reflection;

namespace Assets.Content.Scripts.Others
{
    public class UpgradeManager : MonoBehaviour
    {
        [SerializeField] private PlayerUnit[] _playerUnits;

        public LevelData[] LevelsData;

        public void Upgrade()
        {
            if (MainUI.Instance.CurrentScore >= MainUI.Instance.NeedScore)
            {
                for(int i = 0; i < _playerUnits.Length; i++)
                {
                    _playerUnits[i].gameObject.SetActive(false);
                }
                MainUI.Instance.ResetLevelScore(LevelsData[MainUI.Instance.NeedLevel].Score);
                _playerUnits[MainUI.Instance.CurrentLevel - 1].gameObject.SetActive(true);
            }
        }
    }

    [Serializable]
    public struct LevelData
    {
        public float Level;
        public float Score;
    }
}