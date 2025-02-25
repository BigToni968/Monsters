using Assets.Content.Scripts.UI;
using Assets.Content.Scripts.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Content.Scripts.Statbuff
{
    public class ExpStatsBuff : BaseStatbuff
    {
        public override void Active()
        {
            base.Active();
            MainUI.Instance.BuffExp += value;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            MainUI.Instance.BuffExp -= value;
        }
    }
}