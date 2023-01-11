using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS
{
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Damage")]
        public int currentWeaponDamage = 25;
        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }
        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }
        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                PlayerStatsManager playerStats = other.GetComponent<PlayerStatsManager>();

                if(playerStats != null)
                {
                    playerStats.poiseResetTimer = playerStats.totalPoiseResetTime;
                    playerStats.currentPoiseDefence = playerStats.currentPoiseDefence - poiseBreak;
                    if (playerStats.currentPoiseDefence > poiseBreak)
                    {
                        playerStats.TakeDamageNoAnimation(currentWeaponDamage);
                    }
                    else
                    {
                        playerStats.TakeDamage(currentWeaponDamage);
                    }
                }
            }
            if(other.tag == "Enemy")
            {
                EnemyStatsManager enemyStats = other.GetComponent<EnemyStatsManager>();

                if(enemyStats != null)
                {
                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                    enemyStats.currentPoiseDefence = enemyStats.currentPoiseDefence - poiseBreak;
                    if(enemyStats.currentPoiseDefence > poiseBreak)
                    {
                        enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                    }
                    else
                    {
                        enemyStats. TakeDamage(currentWeaponDamage);
                    }
                }
            }
        }
    }
}