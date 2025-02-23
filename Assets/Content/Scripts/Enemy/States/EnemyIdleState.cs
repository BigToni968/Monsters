using Assets.Content.Scripts.Enemy;
using Assets.Content.Scripts.Enemy.States;
using Assets.Content.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : State
{
    private LayerMask _playerLayer;
    private Collider[] _collidersPlayer;
    private Coroutine _coroutine;

    public EnemyController Controller;

    public EnemyIdleState(StateMachine stateMachine) : base(stateMachine)
    {
        Controller = stateMachine as EnemyController;
    }

    public override void OnFinish()
    {
        Controller.Enemy.StopCoroutine(_coroutine);
    }

    public override void OnStart()
    {
        Debug.Log("State:Idle");
        _playerLayer = LayerMask.GetMask("Player");
        _coroutine = Controller.Enemy.StartCoroutine(Regeneration());
    }

    public override void OnUpdate()
    {
        FindPlayer();
        if (!Controller.Enemy.IsActive)
        {
            Regeneration();
        }
    }

    private void FindPlayer()
    {
        _collidersPlayer = Physics.OverlapSphere(Controller.Enemy.transform.position, Controller.Enemy.Model.RadiusFindPlayer, _playerLayer);

        if (_collidersPlayer.Length > 0)
        {
            Controller.Enemy.InfoUnit.ShowCanvas();

            if (Controller.Enemy.IsActive && !Controller.Enemy.IsDeath)
            {
                Controller.Switch(new EnemyAttackState(Controller));
            }
        }
        else
        {
            Controller.Enemy.InfoUnit.HideCanvas();
        }
    }

    private IEnumerator Regeneration()
    {
        while (true)
        {
            yield return new WaitForSeconds(Controller.Enemy.Model.DelayRegeneration);
            if (!Controller.Enemy.IsActive)
            {
                Controller.Enemy.CurrentHealth += Controller.Enemy.Model.Regeneration;
                Controller.Enemy.CurrentHealth = Mathf.Clamp(Controller.Enemy.CurrentHealth, 0, Controller.Enemy.MaxHealth);
            }
            yield return null;
        }
        
    }
}
