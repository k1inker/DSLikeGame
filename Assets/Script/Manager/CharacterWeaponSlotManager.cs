using UnityEngine;

namespace DS
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        protected CharacterManager _character;

        [Header("Weapons")]
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;

        [Header("Weapon Slots")]
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;

        [Header("Weapon Collider")]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        [Header("Current Item Being Used")]
        public Item currentItemBeingUsed;

        protected virtual void Awake()
        {
            _character = GetComponent<CharacterManager>();
            LoadWeaponHolderSlots();
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
                _character.characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                _character.animator.runtimeAnimatorController = weaponItem.weaponController;
                _character.characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
        }   
        protected virtual void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

            leftHandDamageCollider.currentWeaponDamage = leftWeapon.baseDamage;
            leftHandDamageCollider.poiseBreak = leftWeapon.poiseBreak;

            leftHandDamageCollider.teamIDNumber = _character.characterStatsManager.teamIDNumber;

            _character.characterEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
        protected virtual void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

            rightHandDamageCollider.currentWeaponDamage = rightWeapon.baseDamage;
            rightHandDamageCollider.poiseBreak = rightWeapon.poiseBreak;

            rightHandDamageCollider.teamIDNumber = _character.characterStatsManager.teamIDNumber;

            _character.characterEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
        protected virtual void OpenDamageCollider()
        {
            _character.characterSFXManager.PlayRandomWeaponWhoosh();

            if (_character.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else if (_character.isUsingLeftHand)
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
            WeaponItem currentWeaponBeingUsed = currentItemBeingUsed as WeaponItem;
            _character.characterStatsManager.currentPoiseDefence = _character.characterStatsManager.currentPoiseDefence + currentWeaponBeingUsed.offensivePoiseBonus;
        }
        public virtual void ResetWeaponAttackingPoiseBonus()
        {
            _character.characterStatsManager.currentPoiseDefence = _character.characterStatsManager.totalPoiseDefence;
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
    }
}