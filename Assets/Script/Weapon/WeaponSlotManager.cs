using UnityEngine;

namespace SG
{
    public class WeaponSlotManager : MonoBehaviour
    {
        public WeaponItem attackingWeapon;
        
        private WeaponHolderSlot _leftHandSlot;
        private WeaponHolderSlot _rightHandSlot;


        private DamageCollider _leftHandDamageCollider;
        private DamageCollider _rightHandDamageCollider;

        private PlayerStats _playerStats;
        private void Awake()
        {
            _playerStats = GetComponentInParent<PlayerStats>();

            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if(weaponSlot.isLeftHandSlot)
                {
                    _leftHandSlot = weaponSlot;
                }
                else if(weaponSlot.isRightHandSlot)
                {
                    _rightHandSlot = weaponSlot;
                }
            }
        }
        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if(isLeft)
            {
                _leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
            }
            else
            {
                _rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
            }
        }
        #region Handle Weapon`s Damage Collider
        private void LoadLeftWeaponDamageCollider()
        {
            _leftHandDamageCollider = _leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

        }
        private void LoadRightWeaponDamageCollider()
        {
            _rightHandDamageCollider = _rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }
        public void OpenRightDamageCollider()
        {
            _rightHandDamageCollider.EnableDamageCollider();
        }
        public void OpenLeftDamageCollider()
        {
            _leftHandDamageCollider.EnableDamageCollider();
        }
        public void CloseRightDamageCollider()
        {
            _rightHandDamageCollider.DisableDamageCollider();
        }
        public void CloseLeftDamageCollider()
        {
            _leftHandDamageCollider.DisableDamageCollider();
        }
        #endregion
        #region Handle Weapon`s Stamina Drainage
        public void DrainStaminaLightAttack()
        {
            _playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }
        public void DrainStaminaHeavyAttack()
        {
            _playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }
        #endregion
    }
}