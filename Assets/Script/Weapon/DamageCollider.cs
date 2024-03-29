using UnityEngine;

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

                CharacterManager enemyManager = collision.GetComponent<CharacterManager>();

                _hasBeenParried = false;

                if (enemyManager.isDead || enemyManager.isInvulnerable)
                    return;

                if (enemyManager.characterStatsManager.teamIDNumber == teamIDNumber)
                    return;

                CheckForParry(enemyManager);
                CheckForBlock(enemyManager);

                if(enemyManager.characterStatsManager != null)
                {
                    if(_shieldHasBeenHit)
                        return;
                    if (_hasBeenParried)
                        return;

                    enemyManager.characterStatsManager.poiseResetTimer = enemyManager.characterStatsManager.totalPoiseResetTime;
                    enemyManager.characterStatsManager.currentPoiseDefence = enemyManager.characterStatsManager.currentPoiseDefence - poiseBreak;

                    Vector3 hitPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    enemyManager.characterEffectsManager.PlayBloodSplatterFX(hitPoint);

                    DealDamage(enemyManager.characterStatsManager);
                }
            }
        }
        private void CheckForParry(CharacterManager enemyManager)
        {
            if(enemyManager.isParrying)
            {
                _characterManager.characterAnimatorManager.PlayTargetAnimationWithRootMotion("Parried", true);
                _hasBeenParried = true;
                //for advanced attack ai
                if(!(enemyManager is EnemyManager))
                {
                    _characterManager.isParied = true;
                }    
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
            else if((_characterManager as EnemyManager).isBoss)
            {
                finalDamage = currentWeaponDamage;
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
                finalDamage = finalDamage * weapon.heavyAttackDamageModifier;
            }
        }
    }
}