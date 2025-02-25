using Assets.Content.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Content.Scripts.Statbuff
{
    public class GoldStatBuff : BaseStatbuff
    {
        public override void Active()
        {
            base.Active();
            MainUI.Instance.BuffGold += value;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            MainUI.Instance.BuffGold -= value;
        }
    }
}