using UnityEngine;

namespace DS
{
    public class EnemyWeaponSlotManager : MonoBehaviour
    {
        [SerializeField] private WeaponItem rightHandWeapon;
        [SerializeField] private WeaponItem leftHandWeapon;

        private WeaponHolderSlot _rightHandSlot;
        private WeaponHolderSlot _leftHandSlot;

        private DamageCollider _leftHandDamageCollider;
        private DamageCollider _rightHandDamageCollider;

        private EnemyStats _enemyStats;
        private void Awake()
        {
            _enemyStats = GetComponentInParent<EnemyStats>();
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
                    _leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    _rightHandSlot = weaponSlot;
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
                _leftHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(true);
            }
            else
            {
                _rightHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(false);
            }
        }
        public void LoadWeaponsDamageCollider(bool isLeft)
        {
            if(isLeft)
            {
                _leftHandDamageCollider = _leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
            else
            {
                _rightHandDamageCollider = _rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
        }
        public void OpenDamageCollider()
        {
            _rightHandDamageCollider.EnableDamageCollider();
        }
        public void CloseDamageCollider()
        {
            _rightHandDamageCollider.DisableDamageCollider();
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
            _enemyStats.currentPoiseDefence = _enemyStats.currentPoiseDefence + _enemyStats.offensivePoiseBonus;
        }
        public void ResetWeaponAttackingPoiseBonus()
        {
            _enemyStats.currentPoiseDefence = _enemyStats.totalPoiseDefence;
        }
    }
    
}