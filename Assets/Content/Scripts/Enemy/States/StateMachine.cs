using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Content.Scripts.Enemy.States
{
    public abstract class StateMachine
    {
        public State Current { get; protected set; }

        public abstract void OnUpdate();
        public abstract void Switch(State state);
    }
}