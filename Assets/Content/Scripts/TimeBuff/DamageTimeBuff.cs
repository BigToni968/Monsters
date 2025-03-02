using Assets.Content.Scripts.UI;
using Assets.Content.Scripts.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Content.Scripts.TimeBuff
{
    public class DamageTimeBuff : BaseTimeBuff
    {
        private float tempDamage;
        public override void Active()
        {
            base.Active();
            tempDamage = UnitController.Instance.DamagePlayerStatic + UnitController.Instance.DamagePlayerBuff;
            UnitController.Instance.DamagePlayerBuff += tempDamage;
            MainUI.Instance.SetStat();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            UnitController.Instance.DamagePlayerBuff -= tempDamage;
            MainUI.Instance.SetStat();
        }
    }
}