using UnityEngine;

namespace DS
{
    public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
    {
        [SerializeField] private WeaponItem rightHandWeapon;
        [SerializeField] private WeaponItem leftHandWeapon;

        private EnemyStatsManager _enemyStatsManager;
        private EnemyEffectsManager _enemyEffectsManager;
        private void Awake()
        {
            _enemyStatsManager = GetComponent<EnemyStatsManager>();
            _enemyEffectsManager = GetComponent<EnemyEffectsManager>();
            LoadWeaponHolderSlots();
        }
        private void Start()
        {
            LoadWeaponOnBothHands();
        }
        public override void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
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
                leftHandDamageCollider.teamIDNumber = _enemyStatsManager.teamIDNumber;

                _enemyEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
            }
            else
            {
                rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                rightHandDamageCollider.teamIDNumber = _enemyStatsManager.teamIDNumber;


                _enemyEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
            }
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
        private void LoadWeaponOnBothHands()
        {
            if (rightHandWeapon != null)
            {
                //LoadWeaponSlot(rightHandWeapon, false);
            }
            if (leftHandWeapon != null)
            {
                //LoadWeaponSlot(leftHandWeapon, true);
            }
        }
    }
    
}