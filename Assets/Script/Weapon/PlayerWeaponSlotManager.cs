using UnityEngine;

namespace DS
{
    public class PlayerWeaponSlotManager : MonoBehaviour
    {
        public WeaponItem attackingWeapon;

        private PlayerManager _playerManager;
        private PlayerInvertoryManager _playerInvertoryManager;
        
        private WeaponHolderSlot _leftHandSlot;
        private WeaponHolderSlot _rightHandSlot;

        private Animator _animator;

        private DamageCollider _leftHandDamageCollider;
        private DamageCollider _rightHandDamageCollider;

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
            _leftHandDamageCollider.currentWeaponDamage = _playerInvertoryManager.leftWeapon.baseDamage;
            _leftHandDamageCollider.poiseBreak = _playerInvertoryManager.leftWeapon.poiseBreak;
        }
        private void LoadRightWeaponDamageCollider()
        {
            _rightHandDamageCollider = _rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            _rightHandDamageCollider.currentWeaponDamage = _playerInvertoryManager.rightWeapon.baseDamage;
            _rightHandDamageCollider.poiseBreak = _playerInvertoryManager.rightWeapon.poiseBreak;
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
            if(_rightHandDamageCollider != null)
                _rightHandDamageCollider.DisableDamageCollider();

            if(_leftHandDamageCollider != null)
                _leftHandDamageCollider?.DisableDamageCollider();
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