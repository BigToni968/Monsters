using UnityEngine;

namespace Assets.Content.Scripts.Statbuff
{
    public abstract class BaseStatbuff : MonoUpdater
    {
        [SerializeField] protected float value;

        private void OnEnable()
        {
            
            //Active();
        }

        private void OnDisable()
        {
            Deactivate();
        }
        private void OnDestroy()
        {
           // Deactivate();
        }
        public void SetValue(float newValue)
        {
            value = newValue;
        }

        public virtual void Active() 
        {
            AllUpdate.Add(this);
        }
        public virtual void Deactivate() 
        {
            AllUpdate.Remove(this);
        }
    }
}