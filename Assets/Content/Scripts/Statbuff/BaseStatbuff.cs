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

        public virtual void Active() { }
        public virtual void Deactivate() { }
    }
}