using UnityEngine;

namespace DS
{
    public class PlayerCombatManager : MonoBehaviour
    {
        private PlayerAnimatorManager _playerAnimatorManager;
        private InputHandler _inputHandler;
        private PlayerWeaponSlotManager _playerWeaponSlotManager;
        private PlayerManager _playerManager;
        private PlayerInvertoryManager _playerInvertoryManager;
        private PlayerStatsManager _playerStatsManager;
        private PlayerEffectsManager _playerEffectsManager;
        private BlockingCollider _blockingCollider;

        [Header("Attack Animations")]
        private string OH_Light_Attack_1 = "1_Heavy_Light_attack_01";
        private string OH_Light_Attack_2 = "1_Heavy_Light_attack_02";
        private string OH_Heavy_Attack_1 = "1_Heavy_Heavy_attack_01";
        private string OH_Heavy_Attack_2 = "1_Heavy_Heavy_attack_02";

        public string lastAttack;
        private void Awake()
        {
            _inputHandler = GetComponent<InputHandler>();
            _playerManager = GetComponent<PlayerManager>();
            _playerStatsManager = GetComponent<PlayerStatsManager>();
            _playerEffectsManager = GetComponent<PlayerEffectsManager>();
            _blockingCollider = GetComponentInChildren<BlockingCollider>();
            _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            _playerInvertoryManager = GetComponent<PlayerInvertoryManager>();
            _playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        }
        public void HandleRBAttack()
        {
            PerformRBMeleeAction();
        }
        public void HandleLBAction()
        {
            if (_playerInvertoryManager.leftWeapon.weaponType == WeaponType.Shield)
            {
                PerformLBBlockingAction();
            }
            else if (_playerInvertoryManager.leftWeapon.weaponType == WeaponType.EasySword)
            {
                //do attack
            }
        }
        private void PerformRBMeleeAction()
        {
            if (_playerManager.canDoCombo)
            {
                _inputHandler.comboFlag = true;
                HandleWeaponCombo(_playerInvertoryManager.rightWeapon);
                _inputHandler.comboFlag = false;
            }
            else
            {
                if (_playerManager.isInteracting)
                    return;
                if (_playerManager.canDoCombo)
                    return;

                _playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
                HandleLightAttack(_playerInvertoryManager.rightWeapon);
            }
            _playerEffectsManager.PlayWeaponFX(false);
        }
        private void PerformLBBlockingAction()
        {
            if (_playerManager.isInteracting)
                return;

            if (_playerManager.isBlocking)
                return;

            _playerAnimatorManager.PlayTargetAnimation("Block Start", false, true);
            _blockingCollider.EnableBlockingCollider();
            _playerManager.isBlocking = true;
        }
        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if ((weapon.baseStamina * weapon.lightAttackMultiplier) > _playerStatsManager.currentStamina)
                return;
            if (_inputHandler.comboFlag)
            { 
                _playerAnimatorManager.animator.SetBool("canDoCombo", false);
                if (lastAttack == OH_Light_Attack_1)
                {
                    _playerAnimatorManager.PlayTargetAnimationWithRootMotion(OH_Light_Attack_2, true);
                }
                else if(lastAttack == OH_Heavy_Attack_1)
                {
                    _playerAnimatorManager.PlayTargetAnimationWithRootMotion(OH_Heavy_Attack_2, true);
                }
            }
        }
        public void HandleLightAttack(WeaponItem weapon)
        {
            if ((weapon.baseStamina * weapon.lightAttackMultiplier) > _playerStatsManager.currentStamina)
                return;
            if (weapon == null)
                return;
            _playerWeaponSlotManager.attackingWeapon = weapon;
            _playerAnimatorManager.PlayTargetAnimationWithRootMotion(OH_Light_Attack_1, true);
            lastAttack = OH_Light_Attack_1;
        }
        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (weapon.baseStamina * weapon.heavyAttackMultiplier > _playerStatsManager.currentStamina)
                return;
            if (weapon == null)
                return;
            _playerWeaponSlotManager.attackingWeapon = weapon;
            _playerAnimatorManager.PlayTargetAnimationWithRootMotion(OH_Heavy_Attack_1, true);
            lastAttack = OH_Heavy_Attack_1;
        }
    }
}