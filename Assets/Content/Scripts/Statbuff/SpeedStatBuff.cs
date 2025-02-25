using Assets.Content.Scripts.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Content.Scripts.Statbuff
{
    public class SpeedStatBuff : BaseStatbuff
    {
        public override void Active()
        {
            base.Active();
            UnitController.Instance.BuffSpeed += value;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            UnitController.Instance.BuffSpeed -= value;
        }
    }
}