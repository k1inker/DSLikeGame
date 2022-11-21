using System.Net;
using UnityEngine;

namespace DS
{
    public class PlayerAttacker : MonoBehaviour
    {
        private AnimatorHandler _animatorHandler;
        private InputHandler _inputHandler;
        private WeaponSlotManager _weaponSlotManager;
        public string lastAttack;
        private void Awake()
        {
            _animatorHandler = GetComponentInChildren<AnimatorHandler>();
            _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            _inputHandler = GetComponent<InputHandler>();
        }
        public void HandleWeaponCombo(WeaponItem weapon)
        {
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
            if (weapon == null)
                return;
            _weaponSlotManager.attackingWeapon = weapon;
            _animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true, true);
            lastAttack = weapon.OH_Light_Attack_1;
        }
        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (weapon == null)
                return;
            _weaponSlotManager.attackingWeapon = weapon;
            _animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true, true);
            lastAttack = weapon.OH_Heavy_Attack_1;
        }
    }
}