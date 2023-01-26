using UnityEngine;

namespace DS
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        private PlayerManager _player;

        [Header("Attack Animations")]
        public string OH_Light_Attack_1 = "1_Heavy_Light_attack_01";
        public string OH_Light_Attack_2 = "1_Heavy_Light_attack_02";
        public string OH_Heavy_Attack_1 = "1_Heavy_Heavy_attack_01";
        public string OH_Heavy_Attack_2 = "1_Heavy_Heavy_attack_02";

        public string lastAttack;
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
    }
}