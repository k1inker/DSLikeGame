using UnityEngine;

namespace DS
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        protected CharacterManager _characterManager;
        protected CharacterStatsManager _characterStatsManager;
        protected CharacterEffectsManager _characterEffectsManager;
        protected CharacterAnimatorManager _characterAnimatorManager;

        [Header("Weapons")]
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;
        public WeaponItem attackingWeapon;

        [Header("Weapon Slots")]
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;

        [Header("Weapon Collider")]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
            _characterStatsManager = GetComponent<CharacterStatsManager>();
            _characterEffectsManager = GetComponent<CharacterEffectsManager>();
            _characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            LoadWeaponHolderSlots();
            //LoadWeapon(rightWeapon, leftWeapon);
        }
        private void Start()
        {
            LoadWeapon(rightWeapon, leftWeapon);
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
        public void LoadWeapon(WeaponItem weaponItemRight, WeaponItem weaponItemLeft)
        {
            if (weaponItemLeft != null)
            {
                leftWeapon = weaponItemLeft;
                LoadWeaponOnSlots(weaponItemLeft, true);
            }
            if (weaponItemRight != null)
            {
                rightWeapon = weaponItemRight;
                LoadWeaponOnSlots(weaponItemRight, false);
            }
        }
        protected virtual void LoadWeaponOnSlots(WeaponItem weaponItem, bool isLeft)
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
                _characterAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
            }
            _characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
        }   
        protected virtual void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

            leftHandDamageCollider.currentWeaponDamage = leftWeapon.baseDamage;
            leftHandDamageCollider.poiseBreak = leftWeapon.poiseBreak;

            leftHandDamageCollider.teamIDNumber = _characterStatsManager.teamIDNumber;

            _characterEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
        protected virtual void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

            rightHandDamageCollider.currentWeaponDamage = rightWeapon.baseDamage;
            rightHandDamageCollider.poiseBreak = rightWeapon.poiseBreak;

            rightHandDamageCollider.teamIDNumber = _characterStatsManager.teamIDNumber;

            _characterEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
        protected virtual void OpenDamageCollider()
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
        protected virtual void CloseDamageCollider()
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
        public virtual void GrantWeaponAttackingPoiseBonus()
        {
            _characterStatsManager.currentPoiseDefence = _characterStatsManager.currentPoiseDefence + attackingWeapon.offensivePoiseBonus;
        }
        public virtual void ResetWeaponAttackingPoiseBonus()
        {
            _characterStatsManager.currentPoiseDefence = _characterStatsManager.totalPoiseDefence;
        }
        protected void LoadWeaponOnBothHands()
        {
            if (rightWeapon != null)
            {
                LoadWeaponOnSlots(rightWeapon, false);
            }
            if (leftWeapon != null)
            {
                LoadWeaponOnSlots(leftWeapon, true);
            }
        }
        public virtual void DrainStaminaLightAttack()
        {

        }
        public virtual void DrainStaminaHeavyAttack()
        {

        }
    }
}