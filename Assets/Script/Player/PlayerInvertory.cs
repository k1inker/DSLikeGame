using UnityEngine;

namespace SG
{
    public class PlayerInvertory : MonoBehaviour
    {
        private WeaponSlotManager _weaponSlotManager;

        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;

        private void Awake()
        {
            _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            _weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            _weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }
    }
}