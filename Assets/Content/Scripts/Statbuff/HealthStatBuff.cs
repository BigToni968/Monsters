using Assets.Content.Scripts.Unit;

namespace Assets.Content.Scripts.Statbuff
{
    public class HealthStatBuff : BaseStatbuff
    {
        public override void Active()
        {
            base.Active();
            UnitController.Instance.CurrentHealthPlayer += value;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            UnitController.Instance.CurrentHealthPlayer -= value;
        }
    }
}