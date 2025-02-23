using Assets.Content.Scripts.Enemy;
using Assets.Content.Scripts.Player;
using Assets.Content.Scripts.UI;
using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Content.Scripts.Others
{
    public class DamageBox : MonoBehaviour
    {
        [SerializeField] private DamageNumber _damageNumber;
        private float _damage;

        private void OnTriggerEnter(Collider other)
        {    
            if (other.transform.parent != transform && other.TryGetComponent<IUnit>(out IUnit unit))
            {
                EnemyUnit enemyUnit = unit as EnemyUnit;
                PlayerUnit player = unit as PlayerUnit;
             
                if (enemyUnit != null)
                {
                    if (enemyUnit.IsDeath) return;
                    unit.TakeDamage(_damage, transform);
                    _damageNumber.Spawn(new Vector3(enemyUnit.transform.position.x, _damageNumber.transform.position.y, enemyUnit.transform.position.z), -_damage);
                    if (enemyUnit.CurrentHealth <= 0)
                    {
                        MainUI.Instance.AddScore(enemyUnit.Model.Score, enemyUnit.Model.Money);
                    }
                }
                if (player != null)
                {
                    unit.TakeDamage(_damage);
                    _damageNumber.Spawn(new Vector3(player.transform.position.x, _damageNumber.transform.position.y, player.transform.position.z), -_damage);
                }
            }
        }

        public void SetDamage(float damage)
        {
            _damage = damage;
        }

        private void OnEnable()
        {
            StartCoroutine(Timer());
        }

        private void OnDisable()
        {
            StopCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(0.2f);
            gameObject.SetActive(false);
        }
    }
}