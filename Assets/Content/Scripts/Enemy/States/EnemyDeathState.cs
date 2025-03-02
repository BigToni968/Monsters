using Assets.Content.Scripts.Enemy;
using Assets.Content.Scripts.Enemy.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : State
{
    public EnemyController Controller;

    public EnemyDeathState(StateMachine stateMachine) : base(stateMachine)
    {
        Controller = stateMachine as EnemyController;
    }

    public override void OnFinish()
    {

    }

    public override void OnStart()
    {
        if (!Controller.Enemy.IsPassive)
        {
            Controller.Enemy.Animator.SetTrigger("IsDie");
        }else
        {
            Controller.Enemy.Die();
        }
        Controller.Enemy.StartCoroutine(DelayDeath());
    }

    public override void OnUpdate()
    {

    }

    private IEnumerator DelayDeath()
    {
        yield return new WaitForSeconds(Controller.Enemy.Model.DelayDeath);
        Controller.Enemy.IsDeath = false;
        if (!Controller.Enemy.IsPassive)
        {
            Controller.Enemy.Animator.SetTrigger("IsIdle");
        }
        Controller.Enemy.Rebirth();
        Controller.Switch(new EnemyIdleState(Controller));
    }
}
