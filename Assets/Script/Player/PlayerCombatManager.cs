using UnityEngine;

namespace DS
{
    public class PlayerCombatManager : MonoBehaviour
    {
        private PlayerManager _player;

        [Header("Attack Animations")]
        private string OH_Light_Attack_1 = "1_Heavy_Light_attack_01";
        private string OH_Light_Attack_2 = "1_Heavy_Light_attack_02";
        private string OH_Heavy_Attack_1 = "1_Heavy_Heavy_attack_01";
        private string OH_Heavy_Attack_2 = "1_Heavy_Heavy_attack_02";

        public string lastAttack;
        private void Awake()
        {
            _player = GetComponent<PlayerManager>();
        }
        public void HandleRBAttack()
        {
            if (_player.playerWeaponSlotManager.rightWeapon == null)
                return;
            
            PerformRBMeleeAction();
        }
        public void HandleLBAction()
        {
            if(_player.playerWeaponSlotManager.leftWeapon == null)
                return;

            if (_player.playerWeaponSlotManager.leftWeapon.weaponType == WeaponType.Shield) 
            {
                PerformLBBlockingAction();
            }
            else if (_player.playerWeaponSlotManager.leftWeapon.weaponType == WeaponType.EasySword)
            {
                //do attack
            }
        }
        private void PerformRBMeleeAction()
        {
            if (_player.canDoCombo)
            {
                _player.inputHandler.comboFlag = true;
                HandleWeaponCombo(_player.playerWeaponSlotManager.rightWeapon);
                _player.inputHandler.comboFlag = false;
            }
            else
            {
                if (_player.isInteracting)
                    return;
                if (_player.canDoCombo)
                    return;

                _player.animator.SetBool("isUsingRightHand", true);
                HandleLightAttack(_player.playerWeaponSlotManager.rightWeapon);
            }
            _player.playerEffectsManager.PlayWeaponFX(false);
        }
        private void PerformLBBlockingAction()
        {
            if (_player.isInteracting)
                return;

            if (_player.isBlocking)
                return;

            _player.playerAnimatorManager.PlayTargetAnimation("Block Start", false, true);
            _player.blockingCollider.EnableBlockingCollider();
            _player.isBlocking = true;
        }
        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if ((weapon.baseStamina * weapon.lightAttackMultiplier) > _player.playerStatsManager.currentStamina)
                return;
            if (_player.inputHandler.comboFlag)
            {
                _player.animator.SetBool("canDoCombo", false);
                if (lastAttack == OH_Light_Attack_1)
                {
                    _player.playerAnimatorManager.PlayTargetAnimationWithRootMotion(OH_Light_Attack_2, true);
                }
                else if(lastAttack == OH_Heavy_Attack_1)
                {
                    _player.playerAnimatorManager.PlayTargetAnimationWithRootMotion(OH_Heavy_Attack_2, true);
                }
            }
        }
        public void HandleLightAttack(WeaponItem weapon)
        {
            if ((weapon.baseStamina * weapon.lightAttackMultiplier) > _player.playerStatsManager.currentStamina)
                return;
            if (weapon == null)
                return;
            _player.playerWeaponSlotManager.attackingWeapon = weapon;
            _player.playerAnimatorManager.PlayTargetAnimationWithRootMotion(OH_Light_Attack_1, true);
            lastAttack = OH_Light_Attack_1;
        }
        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (weapon.baseStamina * weapon.heavyAttackMultiplier > _player.playerStatsManager.currentStamina)
                return;
            if (weapon == null)
                return;
            _player.playerWeaponSlotManager.attackingWeapon = weapon;
            _player.playerAnimatorManager.PlayTargetAnimationWithRootMotion(OH_Heavy_Attack_1, true);
            lastAttack = OH_Heavy_Attack_1;
        }
    }
}