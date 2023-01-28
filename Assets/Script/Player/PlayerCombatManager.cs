using UnityEngine;

namespace DS
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        private PlayerManager _player;

        protected override void Awake()
        {
            base.Awake();
            _player = GetComponent<PlayerManager>();
        }
        public override void DrainStaminaBasedOnAttack()
        {
            if(_player.isUsingRightHand)
            {
                if(currentAttackType == AttackType.light)
                {
                    _player.playerStatsManager.DeductStamina(_player.characterWeaponSlotManager.rightWeapon.baseStaminaCost * _player.characterWeaponSlotManager.rightWeapon.lightAttackStaminaMultiplier);
                }
                else if(currentAttackType == AttackType.heavy)
                {
                    _player.playerStatsManager.DeductStamina(_player.characterWeaponSlotManager.rightWeapon.baseStaminaCost * _player.characterWeaponSlotManager.rightWeapon.heavyAttackStaminaMultiplier);
                }
            }
            else if(_player.isUsingLeftHand)
            {
                if (currentAttackType == AttackType.light)
                {
                    _player.playerStatsManager.DeductStamina(_player.characterWeaponSlotManager.leftWeapon.baseStaminaCost * _player.characterWeaponSlotManager.leftWeapon.lightAttackStaminaMultiplier);
                }
                else if (currentAttackType == AttackType.heavy)
                {
                    _player.playerStatsManager.DeductStamina(_player.characterWeaponSlotManager.leftWeapon.baseStaminaCost * _player.characterWeaponSlotManager.leftWeapon.heavyAttackStaminaMultiplier);
                }
            }
        }
        public override void AttemptBlock(DamageCollider attackingWeapon, float damage, string blockAnimation)
        {
            base.AttemptBlock(attackingWeapon, damage, blockAnimation);
            _player.playerStatsManager.staminaBar.SetCurrentStamina(Mathf.RoundToInt(_player.playerStatsManager.currentStamina));
        }
    }
}