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
        public override void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
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