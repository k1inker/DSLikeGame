using UnityEngine;

namespace DS
{
    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
        public WeaponItem attackingWeapon;

        private Animator _animator;
        private PlayerManager _playerManager;
        private PlayerStatsManager _playerStatsManager;
        private PlayerEffectsManager _playerEffectsManager;
        private PlayerAnimatorManager _playerAnimatorManager;
        private PlayerInvertoryManager _playerInvertoryManager;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _playerManager = GetComponent<PlayerManager>();
            _playerStatsManager = GetComponent<PlayerStatsManager>();
            _playerEffectsManager = GetComponent<PlayerEffectsManager>();
            _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
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
                _playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                _playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                _playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
            }
        }

        #region Handle Weapon`s Damage Collider
        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

            leftHandDamageCollider.currentWeaponDamage = _playerInvertoryManager.leftWeapon.baseDamage;
            leftHandDamageCollider.poiseBreak = _playerInvertoryManager.leftWeapon.poiseBreak;

            leftHandDamageCollider.teamIDNumber = _playerStatsManager.teamIDNumber;

            _playerEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

            rightHandDamageCollider.currentWeaponDamage = _playerInvertoryManager.rightWeapon.baseDamage;
            rightHandDamageCollider.poiseBreak = _playerInvertoryManager.rightWeapon.poiseBreak;

            rightHandDamageCollider.teamIDNumber = _playerStatsManager.teamIDNumber;

            _playerEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
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