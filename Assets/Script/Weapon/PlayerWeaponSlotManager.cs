using UnityEngine;

namespace DS
{
    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
        public WeaponItem attackingWeapon;

        private PlayerManager _playerManager;
        private PlayerInvertoryManager _playerInvertoryManager;
        private Animator _animator;

        private PlayerStatsManager _playerStatsManager;
        private void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
            _playerStatsManager = GetComponent<PlayerStatsManager>();
            _animator = GetComponent<Animator>();
            _playerInvertoryManager = GetComponent<PlayerInvertoryManager>();
            LoadWeaponHolderSlots();
        }
        private void LoadWeaponHolderSlots()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if(weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if(weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
            }

        }
        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if(isLeft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
            }
            else
            {
                rightHandSlot.LoadWeaponModel(weaponItem);
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
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.currentWeaponDamage = _playerInvertoryManager.leftWeapon.baseDamage;
            leftHandDamageCollider.poiseBreak = _playerInvertoryManager.leftWeapon.poiseBreak;
        }
        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.currentWeaponDamage = _playerInvertoryManager.rightWeapon.baseDamage;
            rightHandDamageCollider.poiseBreak = _playerInvertoryManager.rightWeapon.poiseBreak;
        }
        public void OpenDamageCollider()
        {
            if (_playerManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else if(_playerManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }
        public void CloseDamageCollider()
        {
            if(rightHandDamageCollider != null)
                rightHandDamageCollider.DisableDamageCollider();

            if(leftHandDamageCollider != null)
                leftHandDamageCollider?.DisableDamageCollider();
        }
        #endregion

        #region Handle Weapon`s Stamina Drainage
        public void DrainStaminaLightAttack()
        {
            _playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }
        public void DrainStaminaHeavyAttack()
        {
            _playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }
        #endregion

        public void GrantWeaponAttackingPoiseBonus()
        {
            _playerStatsManager.currentPoiseDefence = _playerStatsManager.currentPoiseDefence + attackingWeapon.offensivePoiseBonus;
        }
        public void ResetWeaponAttackingPoiseBonus()
        {
            _playerStatsManager.currentPoiseDefence = _playerStatsManager.totalPoiseDefence;
        }
    }
}