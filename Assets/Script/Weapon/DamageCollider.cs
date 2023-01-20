using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace DS
{
    public class DamageCollider : MonoBehaviour
    {
        private Collider damageCollider;
        private CharacterManager _character;

        [Header("Team I.D")]
        public int teamIDNumber = 0;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Damage")]
        public int currentWeaponDamage = 25;

        private bool _shieldHasBeenHit;
        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }
        public void EnableDamageCollider()
        {
            _character = GetComponentInParent<CharacterManager>();
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

                CharacterStatsManager characterStatsManager = collision.GetComponent<CharacterStatsManager>();
                CharacterManager characterManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();
                CharacterEffectsManager playerEffectsManager = collision.GetComponent <CharacterEffectsManager>();

                if (characterStatsManager.teamIDNumber == teamIDNumber)
                    return;

                CheckForBlock(characterManager, shield, characterStatsManager);

                if(characterStatsManager != null)
                {
                    if(_shieldHasBeenHit)
                        return;

                    characterStatsManager.poiseResetTimer = characterStatsManager.totalPoiseResetTime;
                    characterStatsManager.currentPoiseDefence = characterStatsManager.currentPoiseDefence - poiseBreak;

                    Vector3 hitPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    playerEffectsManager.PlayBloodSplatterFX(hitPoint);

                    DealDamage(characterStatsManager);
                }
            }
        }
        private void CheckForBlock(CharacterManager characterManager, BlockingCollider shield, CharacterStatsManager characterStatsManager)
        {
            if (characterManager != null)
            {
                if (shield != null && characterManager.isBlocking && characterStatsManager != null)
                {
                    characterStatsManager.TakeDamage(0, "Block Guard");
                    _shieldHasBeenHit = true;
                    //if(characterStatsManager.currentStamina <= 0)
                    //{
                    //    characterStatsManager.TakeDamage(0, "Destroy Block Guard");
                    //}
                    //characterStatsManager.TakeStaminaDamage(15);
                    return;
                }
            }
        }
        private void DealDamage(CharacterStatsManager characterStatsManager)
        {
            float finalDamage = currentWeaponDamage;
            if(_character.isUsingRightHand)
            {
                CheckAttackType(_character.characterWeaponSlotManager.rightWeapon, ref finalDamage);
            }
            else if(_character.isUsingLeftHand)
            {
                CheckAttackType(_character.characterWeaponSlotManager.leftWeapon, ref finalDamage);
            }


            if (characterStatsManager.currentPoiseDefence > poiseBreak)
            {
                characterStatsManager.TakeDamageNoAnimation(Mathf.RoundToInt(finalDamage));
            }
            else
            {
                characterStatsManager.TakeDamage(Mathf.RoundToInt(finalDamage));
                characterStatsManager.currentPoiseDefence = characterStatsManager.totalPoiseDefence;
            }
        }

        private void CheckAttackType(WeaponItem weapon, ref float finalDamage)
        {
            if (_character.characterCombatManager.currentAttackType == AttackType.light)
            {
                finalDamage = finalDamage * weapon.lightAttackDamageModifier;
            }
            else if(_character.characterCombatManager.currentAttackType == AttackType.heavy)
            {
                Debug.Log(1);
                finalDamage = finalDamage * weapon.heavyAttackDamageModifier;
            }
        }
    }
}