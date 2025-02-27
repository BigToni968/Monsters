using Assets.Content.Scripts.UI;

namespace Assets.Content.Scripts.TimeBuff
{
    public class GoldTimeBuff : BaseTimeBuff
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