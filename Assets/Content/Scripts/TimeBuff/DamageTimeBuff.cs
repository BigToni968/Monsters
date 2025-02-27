using Assets.Content.Scripts.UI;
using Assets.Content.Scripts.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Content.Scripts.TimeBuff
{
    public class DamageTimeBuff : BaseTimeBuff
    {
        public override void Active()
        {
            base.Active();
            UnitController.Instance.DamagePlayer *= value;
            MainUI.Instance.SetStat();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            UnitController.Instance.DamagePlayer /= value;
            MainUI.Instance.SetStat();
        }
    }
}