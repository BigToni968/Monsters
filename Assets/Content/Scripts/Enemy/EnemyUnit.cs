using Assets.Content.Scripts.Others;
using Assets.Content.Scripts.UI;
using Assets.Content.Scripts.Unit;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using Random = UnityEngine.Random;

namespace Assets.Content.Scripts.Enemy
{
    public class EnemyUnit : MonoUpdater, IUnit
    {
        [SerializeField] private EnemyController _controller;
        [SerializeField] private DamageBox _damageBox;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField] private Material _highlightMaterial;
        [SerializeField] private ParticleSystem _hitEffects;

        public ModelUnit Model;
        public string[] names;
        public Animator Animator;
        public WindowInfoUnit InfoUnit;
        public float CurrentHealth;
        public float MaxHealth;
        public bool IsActive;
        public bool IsDeath;
        public bool IsPassive;
        public bool IsBossMap1 = false;
        public bool IsBossMap2 = false;
        public bool IsBossMap3 = false;
        public Transform Target;

        private Material[] _originalMaterials;

        private void Start()
        {
            if (YandexGame.EnvironmentData.language == "ru")
            {
                InfoUnit.SetName(names[0]);
            }
            else if (YandexGame.EnvironmentData.language == "en")
            {
                InfoUnit.SetName(names[1]);
            }
            else if (YandexGame.EnvironmentData.language == "tr")
            {
                InfoUnit.SetName(names[2]);
            }
            SetOptions();
            _controller = new EnemyController(this);
            _controller.Switch(new EnemyIdleState(_controller));

            if (_meshRenderer != null)
            {
                _originalMaterials = _meshRenderer.materials;
            }
            else if (_skinnedMeshRenderer != null)
            {
                _originalMaterials = _skinnedMeshRenderer.materials;
            }
        }
        public override void OnTick()
        {
            _controller?.OnUpdate();
            InfoUnit.SetHealth(CurrentHealth, MaxHealth);
        }

        public void TakeDamage(float damage, Transform transform = null)
        {
            AudioManager.Instance.Sound.PlayOneShot(AudioManager.Instance.KickClips[Random.Range(0, AudioManager.Instance.KickClips.Length)]);
            TakeHit();
            IsActive = true;
            Target = transform;
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);
            if (CurrentHealth <= 0)
            {
                if (IsBossMap1)
                {
                    YandexGame.savesData.IsBossDeathMap1 = true;
                    YandexGame.SaveProgress();
                }
                if (IsBossMap2)
                {
                    YandexGame.savesData.IsBossDeathMap2 = true;
                    YandexGame.SaveProgress();
                }
                if (IsBossMap3)
                {
                    YandexGame.savesData.IsBossDeathMap3 = true;
                    YandexGame.SaveProgress();
                }

                IsDeath = true;
                IsActive = false;
                Target = null;
                _controller.Switch(new EnemyDeathState(_controller));
            }
        }

        public void Die()
        {
            InfoUnit.HideCanvas();
            _boxCollider.isTrigger = true;
            if (_meshRenderer != null)
            {
                _meshRenderer.enabled = false;
            }
            else if (_skinnedMeshRenderer != null)
            {
                _skinnedMeshRenderer.enabled = false;
            }
        }

        public void DieForActive()
        {
            InfoUnit.HideCanvas();
        }

        public void Rebirth()
        {
            SetOptions();
            _boxCollider.isTrigger = false;
            if (_meshRenderer != null)
            {
                _meshRenderer.enabled = true;
            }
            else if (_skinnedMeshRenderer != null)
            {
                _skinnedMeshRenderer.enabled = true;
            }
        }

        private void SetOptions()
        {
            CurrentHealth = Model.Health;
            MaxHealth = Model.Health;
        }

        public void ActivateDamageBox()
        {
            _damageBox.SetDamage(Model.Damage);
            _damageBox.gameObject.SetActive(true);
        }

        private void TakeHit()
        {
            _hitEffects.Play();
            StartCoroutine(FlashRed(0.2f));
        }

        private IEnumerator FlashRed(float duration)
        {
            Material[] highlightMaterials = new Material[_originalMaterials.Length];

            for (int i = 0; i < _originalMaterials.Length; i++)
            {
                highlightMaterials[i] = _highlightMaterial;
            }


            if (_meshRenderer != null)
            {
                _meshRenderer.materials = highlightMaterials;
            }
            else if (_skinnedMeshRenderer != null)
            {
                _skinnedMeshRenderer.materials = highlightMaterials;
            }

            yield return new WaitForSeconds(duration);

            if (_meshRenderer != null)
            {
                _meshRenderer.materials = _originalMaterials;
            }
            else if (_skinnedMeshRenderer != null)
            {
                _skinnedMeshRenderer.materials = _originalMaterials;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, Model.RadiusFindPlayer);
        }
    }

    [Serializable]
    public struct ModelUnit
    {
        public float Health;
        public float Damage;
        public float Regeneration;
        public float DelayRegeneration;
        public float RadiusFindPlayer;
        public float DelayAttack;
        public float SpeedRotation;
        public float TimeForAttack;
        public float DelayDeath;
        public float Score;
        public float Money;
    }
}