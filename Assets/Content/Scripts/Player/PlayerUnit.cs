using Assets.Content.Scripts.Others;
using Assets.Content.Scripts.UI;
using Assets.Content.Scripts.Unit;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Content.Scripts.Player
{
    public class PlayerUnit : MonoBehaviour, IUnit
    {
        [SerializeField] private DamageBox _damageBox;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _delayRelax;
        [SerializeField] private int _index;
        [SerializeField] private Transform _unit;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private BoxCollider _boxCollider;

        public Animator Animator;
        public ModelUnit Model;
        public bool _autoAttack = false;
        public Sprite Sprite;

        private bool _takeDamage = false;
        private Coroutine _coroutineRelax;

        private void Start()
        {
            StartCoroutine(Regeneration());
        }

        private void Update()
        {
            Attack();
            SetRelax();
        }

        private void Attack()
        {
            if (MainUI.Instance.IsCanvasEnable()) return;
            if (MainUI.Instance.AutoAttack)
            {
                Animator.SetTrigger("IsAttack");
                return;
            }
            if (UnitController.Instance.IsMobile) return;
            if (Input.GetMouseButtonDown(0))
            {
                Animator.SetTrigger("IsAttack");
            }
        }

        public void ActivateDamageBox()
        {
            _damageBox.SetDamage(UnitController.Instance.DamagePlayerStatic + UnitController.Instance.DamagePlayerBuff);
            _damageBox.gameObject.SetActive(true);
        }

        public void TakeDamage(float damage, Transform transform = null)
        {
            AudioManager.Instance.Sound.PlayOneShot(AudioManager.Instance.KickClips[Random.Range(0, AudioManager.Instance.KickClips.Length)]);
            if (_coroutineRelax != null)
            {
                StopCoroutine(_coroutineRelax);
                _coroutineRelax = null;
            }
            UnitController.Instance.IsTakeDamage = true;
            UnitController.Instance.CurrentHealthPlayer -= damage;
            UnitController.Instance.CurrentHealthPlayer = Mathf.Clamp(UnitController.Instance.CurrentHealthPlayer, 0, UnitController.Instance.MaxHealthPlayer);
            UnitController.Instance.InfoUnit.ShowHealth();
            if (UnitController.Instance.CurrentHealthPlayer <= 0)
            {
                Death();
            }
        }

        private void Death()
        {
            UnitController.Instance.CurrentHealthPlayer = UnitController.Instance.MaxHealthPlayer;
            _unit.position = _spawnPoint.position;
        }

        private IEnumerator TimerRelax()
        {
            yield return new WaitForSeconds(_delayRelax);
            UnitController.Instance.IsTakeDamage = false;
            _coroutineRelax = null;
        }

        private IEnumerator Regeneration()
        {
            while (true)
            {
                if (!UnitController.Instance.IsTakeDamage)
                {
                    yield return new WaitForSeconds(1);
                    if (!UnitController.Instance.IsTakeDamage)
                    {
                        UnitController.Instance.CurrentHealthPlayer += Model.Regeneration;
                        UnitController.Instance.CurrentHealthPlayer = Mathf.Clamp(UnitController.Instance.CurrentHealthPlayer, 0, UnitController.Instance.MaxHealthPlayer);
                    }
                }
                if (UnitController.Instance.CurrentHealthPlayer >= UnitController.Instance.MaxHealthPlayer)
                {
                    UnitController.Instance.InfoUnit.HideHealth();
                }
                yield return null;
            }
        }

        private void SetRelax()
        {
            if (UnitController.Instance.CurrentHealthPlayer < UnitController.Instance.MaxHealthPlayer)
            {
                if (_coroutineRelax == null)
                {
                    _coroutineRelax = StartCoroutine(TimerRelax());
                }
            }
        }

        public void Deactivate()
        {
            _meshRenderer.enabled = false;
            _boxCollider.enabled = false;
            enabled = false;
        }
        public void Activate()
        {
            UnitController.Instance.SetUnit(this);
            _meshRenderer.enabled = true;
            _boxCollider.enabled = true;
            enabled = true;
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