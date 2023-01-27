using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace DS
{
    public class DamageCollider : MonoBehaviour
    {
        private Collider damageCollider;
        private CharacterManager _characterManager;

        [Header("Team I.D")]
        public int teamIDNumber = 0;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Damage")]
        public int currentWeaponDamage = 25;

        [Header("Guard Break Modifier")]
        public float guardBreakModifier = 1;

        private bool _shieldHasBeenHit;
        private bool _hasBeenParried;
        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }
        private void Start()
        {
            _characterManager = GetComponentInParent<CharacterManager>();
        }
        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }
        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }
        private void OnTriggerEnter(Collider collision)
        {
            if(collision.tag == "Character")
            {
                _shieldHasBeenHit = false;
                _hasBeenParried = false;

                CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();
                CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
                CharacterEffectsManager playerEffectsManager = collision.GetComponent <CharacterEffectsManager>();

                if (enemyStats.teamIDNumber == teamIDNumber)
                    return;

                CheckForParry(enemyManager);
                CheckForBlock(enemyManager);

                if(enemyStats != null)
                {
                    if(_shieldHasBeenHit)
                        return;
                    if (_hasBeenParried)
                        return;

                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                    enemyStats.currentPoiseDefence = enemyStats.currentPoiseDefence - poiseBreak;

                    Vector3 hitPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    playerEffectsManager.PlayBloodSplatterFX(hitPoint);

                    DealDamage(enemyStats);
                }
            }
        }
        private void CheckForParry(CharacterManager enemyManager)
        {
            if(enemyManager.isParrying)
            {
                _characterManager.characterAnimatorManager.PlayTargetAnimation("Parried", true);
                _hasBeenParried = true;
            }
        }
        private void CheckForBlock(CharacterManager enemyManager)
        {
            Vector3 directionFromPlayerToEnemy = _characterManager.transform.position - enemyManager.transform.position;
            float dorValueFromPlayerToEnemy = Vector3.Dot(directionFromPlayerToEnemy, enemyManager.transform.forward);

            if (enemyManager.isBlocking && dorValueFromPlayerToEnemy > 0.3f)
            {
                enemyManager.characterCombatManager.AttemptBlock(this, currentWeaponDamage,"Block Guard");
                _shieldHasBeenHit = true;
            }
        }
        private void DealDamage(CharacterStatsManager enemyStats)
        {
            float finalDamage = currentWeaponDamage;
            if(_characterManager.isUsingRightHand)
            {
                CheckAttackType(_characterManager.characterWeaponSlotManager.rightWeapon, ref finalDamage);
            }
            else if(_characterManager.isUsingLeftHand)
            {
                CheckAttackType(_characterManager.characterWeaponSlotManager.leftWeapon, ref finalDamage);
            }


            if (enemyStats.currentPoiseDefence > poiseBreak)
            {
                enemyStats.TakeDamageNoAnimation(Mathf.RoundToInt(finalDamage));
            }
            else
            {
                enemyStats.TakeDamage(Mathf.RoundToInt(finalDamage));
                enemyStats.currentPoiseDefence = enemyStats.totalPoiseDefence;
            }
        }
        private void CheckAttackType(WeaponItem weapon, ref float finalDamage)
        {
            if (_characterManager.characterCombatManager.currentAttackType == AttackType.light)
            {
                finalDamage = finalDamage * weapon.lightAttackDamageModifier;
            }
            else if(_characterManager.characterCombatManager.currentAttackType == AttackType.heavy)
            {
                Debug.Log(1);
                finalDamage = finalDamage * weapon.heavyAttackDamageModifier;
            }
        }
    }
}