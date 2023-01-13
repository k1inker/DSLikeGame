using UnityEngine;

namespace DS
{
    public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
    {
        [SerializeField] private WeaponItem rightHandWeapon;
        [SerializeField] private WeaponItem leftHandWeapon;

        private EnemyStatsManager _enemyStatsManager;
        private void Awake()
        {
            _enemyStatsManager = GetComponent<EnemyStatsManager>();
            LoadWeaponHolderSlots();
        }
        private void Start()
        {
            LoadWeaponOnBothHands();
        }
        private void LoadWeaponHolderSlots()
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
        private void LoadWeaponOnBothHands()
        {
            if (rightHandWeapon != null)
            {
                LoadWeaponSlot(rightHandWeapon, false);
            }
            if (leftHandWeapon != null)
            {
                LoadWeaponSlot(leftHandWeapon, true);
            }
        }
        public void LoadWeaponSlot(WeaponItem weapon, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(true);
            }
            else
            {
                rightHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(false);
            }
        }
        public void LoadWeaponsDamageCollider(bool isLeft)
        {
            if(isLeft)
            {
                leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
            else
            {
                rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
        }
        public void OpenDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }
        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }
        public void DrainStaminaLightAttack()
        {

        }
        public void DrainStaminaHeavyAttack()
        {
            
        }
        public void EnableCombo()
        {

        }
        public void DisableCombo()
        {

        }
        public void GrantWeaponAttackingPoiseBonus()
        {
            _enemyStatsManager.currentPoiseDefence = _enemyStatsManager.currentPoiseDefence + _enemyStatsManager.offensivePoiseBonus;
        }
        public void ResetWeaponAttackingPoiseBonus()
        {
            _enemyStatsManager.currentPoiseDefence = _enemyStatsManager.totalPoiseDefence;
        }
    }
    
}