using Assets.Content.Scripts.Statbuff;
using UnityEngine;
using System;

namespace Assets.Content.Scripts.TimeBuff
{
    public abstract class BaseTimeBuff : BaseStatbuff
    {
        public float Duration;
        private float _time;

        public event Action<float> OnTime;
        public event Action Removed;

        private bool _start =false;

        public void Play()
        {
            _start = true;
            Active();
        }

        public override void Active()
        {
            base.Active();
            _time = Duration;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            Removed?.Invoke();
        }


        private void Update()
        {
            if (!_start)
                return;

            _time -= Time.deltaTime;
            _time = Mathf.Clamp(_time, 0f, Duration);
            OnTime?.Invoke(_time);

            if (_time <= 0f)
                Destroy(this);
        }
    }
}