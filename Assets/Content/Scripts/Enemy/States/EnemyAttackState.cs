using Assets.Content.Scripts.Enemy;
using Assets.Content.Scripts.Enemy.States;
using Assets.Content.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : State
{
    public EnemyController Controller;

    private Coroutine _coroutine;
    private Coroutine _coroutineStopAttack;
    private Collider[] _collidersPlayer;
    private bool _isRange;
    private LayerMask _playerLayer;
    private bool _isSetTarget;
    private Vector3 _targetPos;

    public EnemyAttackState(StateMachine stateMachine) : base(stateMachine)
    {
        Controller = stateMachine as EnemyController;
    }

    public override void OnFinish()
    {
        if (!Controller.Enemy.IsPassive)
        {
            Controller.Enemy.StopCoroutine(_coroutine);
            _coroutine = null;
        }

        Controller.Enemy.StopCoroutine(_coroutineStopAttack);
    }

    public override void OnStart()
    {
        Debug.Log("State:Attack");
        _playerLayer = LayerMask.GetMask("Player");
        if (!Controller.Enemy.IsPassive)
        {
            _coroutine = Controller.Enemy.StartCoroutine(Attack());
        }

        _coroutineStopAttack = Controller.Enemy.StartCoroutine(StopAttack());
    }

    public override void OnUpdate()
    {
        CheckRangePlayer();
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(Controller.Enemy.Model.DelayAttack);
            if (_isRange)
            {
                if (!_isSetTarget)
                {
                    _targetPos = Controller.Enemy.Target.transform.position;
                    _isSetTarget = true;
                }

                while (!SetLookAt(_targetPos))
                {
                    yield return null;
                }
                Controller.Enemy.Animator.SetTrigger("IsAttack");
                _isSetTarget = false;
            }
        }
    }

    private bool SetLookAt(Vector3 target)
    {
        Vector3 direction = target - Controller.Enemy.transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Controller.Enemy.transform.rotation = Quaternion.Slerp(
                Controller.Enemy.transform.rotation,
                targetRotation,
                Controller.Enemy.Model.SpeedRotation * Time.deltaTime
            );

            return Quaternion.Angle(Controller.Enemy.transform.rotation, targetRotation) < 0.1f;
        }

        return true;
    }

    private IEnumerator StopAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(Controller.Enemy.Model.TimeForAttack);

            if (!_isRange)
            {
                Controller.Enemy.IsActive = false;
                Controller.Enemy.Target = null;

                if (!Controller.Enemy.IsDeath)
                {
                    Controller.Switch(new EnemyIdleState(Controller));
                }
            }
            yield return null;
        }

    }

    private void CheckRangePlayer()
    {
        _collidersPlayer = Physics.OverlapSphere(Controller.Enemy.transform.position, Controller.Enemy.Model.RadiusFindPlayer, _playerLayer);

        if (_collidersPlayer.Length > 0)
        {
            _isRange = true;
        }
        else
        {
            _isRange = false;
        }
    }
}
