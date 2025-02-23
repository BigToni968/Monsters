using Assets.Content.Scripts.Others;
using Assets.Content.Scripts.UI;
using Assets.Content.Scripts.Unit;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Content.Scripts.Player
{
    public class PlayerUnit : MonoBehaviour, IUnit
    {
        [SerializeField] private UnitController _unitController;
        [SerializeField] private Animator _animator;
        [SerializeField] private DamageBox _damageBox;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _delayRelax;
        [SerializeField] private int _index;
        [SerializeField] private Transform _unit;

        public MainUI UI;
        public ModelUnit Model;
        public bool _autoAttack = false;

        private bool _takeDamage = false;
        private bool _isOpen = false;
        private Coroutine _coroutineRelax;

        private void Start()
        {
            if(!_isOpen)
            {
                _isOpen = true;
                _unitController.MaxHealthPlayer += Model.Health;
                _unitController.DamagePlayer += Model.Damage;
            }           
            _unitController.CurrentHealthPlayer = _unitController.MaxHealthPlayer;

            StartCoroutine(Regeneration());
        }

        private void Update()
        {
            Attack();
            SetRelax();
        }

        private void Attack()
        {
            if (Input.GetMouseButtonDown(0) || MainUI.Instance.AutoAttack)
            {
                _animator.SetTrigger("IsAttack");
            }
        }

        public void ActivateDamageBox()
        {
            _damageBox.SetDamage(_unitController.DamagePlayer);
            _damageBox.gameObject.SetActive(true);
        }

        public void TakeDamage(float damage, Transform transform = null)
        {
            if (_coroutineRelax != null)
            {
                StopCoroutine(_coroutineRelax);
                _coroutineRelax = null;
            }

            _takeDamage = true;
            _unitController.CurrentHealthPlayer -= damage;
            _unitController.CurrentHealthPlayer = Mathf.Clamp(_unitController.CurrentHealthPlayer, 0, _unitController.MaxHealthPlayer);
            _unitController.InfoUnit.ShowHealth();
            if (_unitController.CurrentHealthPlayer <= 0)
            {
                Death();
            }
        }

        private void Death()
        {
            _unitController.CurrentHealthPlayer = _unitController.MaxHealthPlayer;
            _unit.position = _spawnPoint.position;
        }

        private IEnumerator TimerRelax()
        {
            yield return new WaitForSeconds(_delayRelax);
            _takeDamage = false;
            _coroutineRelax = null;
        }

        private IEnumerator Regeneration()
        {
            while (true)
            {
                if (!_takeDamage)
                {
                    yield return new WaitForSeconds(1);
                    _unitController.CurrentHealthPlayer += Model.Regeneration;
                    _unitController.CurrentHealthPlayer = Mathf.Clamp(_unitController.CurrentHealthPlayer, 0, _unitController.MaxHealthPlayer);             
                }
                if (_unitController.CurrentHealthPlayer >= _unitController.MaxHealthPlayer)
                {
                    _unitController.InfoUnit.HideHealth();
                }
                yield return null;
            }
        }

        private void SetRelax()
        {
            if (_unitController.CurrentHealthPlayer < _unitController.MaxHealthPlayer)
            {
                if (_coroutineRelax == null)
                {
                    _coroutineRelax = StartCoroutine(TimerRelax());
                }
            }
        }
    }

    [Serializable]
    public struct ModelUnit
    {
        public float Health;
        public float Damage;
        public float Regeneration;
    }
}