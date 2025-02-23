using Assets.Content.Scripts.Enemy.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Content.Scripts.Enemy
{
    public class EnemyController : StateMachine
    {
        public EnemyUnit Enemy;

        public EnemyController(EnemyUnit enemy)
        {
            Enemy = enemy;
        }
        public override void OnUpdate()
        {
            Current?.OnUpdate();
        }

        public override void Switch(State state)
        {
            Current?.OnFinish();
            Current = state;
            Current?.OnStart();
        }
    }
}