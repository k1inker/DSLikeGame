using System.Net;
using UnityEngine;

namespace DS
{
    public class PlayerAttacker : MonoBehaviour
    {
        private PlayerAnimatorManager _animatorHandler;
        private InputHandler _inputHandler;
        private WeaponSlotManager _weaponSlotManager;
        private PlayerManager _playerManager;
        private PlayerInvertory _playerInvertory;
        private PlayerStats _playerStats;
        public string lastAttack;
        private void Awake()
        {
            _animatorHandler = GetComponent<PlayerAnimatorManager>();
            _weaponSlotManager = GetComponent<WeaponSlotManager>();
            _inputHandler = GetComponentInParent<InputHandler>();
            _playerInvertory = GetComponentInParent<PlayerInvertory>();
            _playerManager = GetComponentInParent<PlayerManager>();
            _playerStats = GetComponentInParent<PlayerStats>();
        }
        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if ((weapon.baseStamina * weapon.lightAttackMultiplier) > _playerStats.currentStamina)
                return;
            if (_inputHandler.comboFlag)
            { 
                _animatorHandler.anim.SetBool("canDoCombo", false);
                if (lastAttack == weapon.OH_Light_Attack_1)
                {
                    _animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true, true);
                }
            }
        }
        public void HandleLightAttack(WeaponItem weapon)
        {
            if ((weapon.baseStamina * weapon.lightAttackMultiplier) > _playerStats.currentStamina)
                return;
            if (weapon == null)
                return;
            _weaponSlotManager.attackingWeapon = weapon;
            _animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true, true);
            lastAttack = weapon.OH_Light_Attack_1;
        }
        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (weapon.baseStamina * weapon.heavyAttackMultiplier > _playerStats.currentStamina)
                return;
            if (weapon == null)
                return;
            _weaponSlotManager.attackingWeapon = weapon;
            _animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true, true);
            lastAttack = weapon.OH_Heavy_Attack_1;
        }
        public void HandleRBAttack()
        {
            if (_playerManager.canDoCombo)
            {
                _inputHandler.comboFlag = true;
                HandleWeaponCombo(_playerInvertory.rightWeapon);
                _inputHandler.comboFlag = false;
            }
            else
            {
                if (_playerManager.isInteracting)
                    return;
                if (_playerManager.canDoCombo)
                    return;

                _animatorHandler.anim.SetBool("isUsingRightHand", true);
                HandleLightAttack(_playerInvertory.rightWeapon);
            }
        }
    }
}