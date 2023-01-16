using UnityEngine;

namespace DS
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        private CharacterManager _characterManager;
        private CharacterStatsManager _characterStatsManager;
        private CharacterEffectsManager _characterEffectsManager;
        private CharacterAnimatorManager _characterAnimatorManager;

        [Header("Weapon Slots")]
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;

        [Header("Weapon Collider")]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;
        private void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
            _characterStatsManager = GetComponent<CharacterStatsManager>();
            _characterEffectsManager = GetComponent<CharacterEffectsManager>();
            _characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        }
        protected virtual void LoadWeaponHolderSlots()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
            }
        }
        public virtual void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
            }
            else
            {
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                _characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                _characterAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
            }
        }   
        protected virtual void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

            //leftHandDamageCollider.currentWeaponDamage = _playerInvertoryManager.leftWeapon.baseDamage;
            //leftHandDamageCollider.poiseBreak = _playerInvertoryManager.leftWeapon.poiseBreak;

            leftHandDamageCollider.teamIDNumber = _characterStatsManager.teamIDNumber;

            _characterEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
        protected virtual void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

            //rightHandDamageCollider.currentWeaponDamage = _playerInvertoryManager.rightWeapon.baseDamage;
            //rightHandDamageCollider.poiseBreak = _playerInvertoryManager.rightWeapon.poiseBreak;

            rightHandDamageCollider.teamIDNumber = _characterStatsManager.teamIDNumber;

            _characterEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
        public void OpenDamageCollider()
        {
            if (_characterManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else if (_characterManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }
        public void CloseDamageCollider()
        {
            if (rightHandDamageCollider != null)
            {
                rightHandDamageCollider.DisableDamageCollider();
            }

            if (leftHandDamageCollider != null)
            {
                leftHandDamageCollider?.DisableDamageCollider();
            }
        }
    }
}