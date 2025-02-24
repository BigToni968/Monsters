using UnityEngine;

namespace Assets.Content.Scripts.Statbuff
{
    public abstract class BaseStatbuff : MonoBehaviour
    {
        [SerializeField] protected float value;

        private void OnEnable()
        {
            Active();
        }

        private void OnDisable()
        {
            Deactivate();
        }

        public void SetValue(float newValue)
        {
            value = newValue;
        }

        public virtual void Active() { }
        public virtual void Deactivate() { }
    }
}