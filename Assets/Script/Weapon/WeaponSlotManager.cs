using UnityEngine;

namespace DS
{
    public class WeaponSlotManager : MonoBehaviour
    {
        public WeaponItem attackingWeapon;

        private PlayerManager _playerManager;
        private PlayerInvertory _playerInvertory;
        
        private WeaponHolderSlot _leftHandSlot;
        private WeaponHolderSlot _rightHandSlot;

        private Animator _animator;

        private DamageCollider _leftHandDamageCollider;
        private DamageCollider _rightHandDamageCollider;

        private PlayerStats _playerStats;
        private void Awake()
        {
            _playerManager = GetComponentInParent<PlayerManager>();
            _playerStats = GetComponentInParent<PlayerStats>();
            _animator = GetComponentInParent<Animator>();
            _playerInvertory = GetComponentInParent<PlayerInvertory>();

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
                #region Handle Right Weapon Idle Animation
                if (weaponItem != null)
                {
                    _animator.CrossFade(weaponItem.hold_idle, 0.2f);
                }
                else
                {
                    _animator.CrossFade("Right Arm Empty", 0.2f);
                }
                #endregion
            }
        }
        #region Handle Weapon`s Damage Collider
        private void LoadLeftWeaponDamageCollider()
        {
            _leftHandDamageCollider = _leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            _leftHandDamageCollider.currentWeaponDamage = _playerInvertory.leftWeapon.baseDamage;
        }
        private void LoadRightWeaponDamageCollider()
        {
            _rightHandDamageCollider = _rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            _rightHandDamageCollider.currentWeaponDamage = _playerInvertory.rightWeapon.baseDamage;
        }
        public void OpenDamageCollider()
        {
            if (_playerManager.isUsingRightHand)
            {
                _rightHandDamageCollider.EnableDamageCollider();
            }
            else if(_playerManager.isUsingLeftHand)
            {
                _leftHandDamageCollider.EnableDamageCollider();
            }
        }
        public void CloseDamageCollider()
        {
            _rightHandDamageCollider.DisableDamageCollider();
            _leftHandDamageCollider?.DisableDamageCollider();
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