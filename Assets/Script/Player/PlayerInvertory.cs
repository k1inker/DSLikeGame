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
        public void LoadWeapon(WeaponItem weaponItemRight, WeaponItem weaponItemLeft)
        {
            if (weaponItemLeft != null)
            {
                leftWeapon = weaponItemLeft;
                _weaponSlotManager.LoadWeaponOnSlot(weaponItemLeft, true);
            }
            if (weaponItemRight != null)
            {
                rightWeapon = weaponItemRight;
                _weaponSlotManager.LoadWeaponOnSlot(weaponItemRight, false);
            }
        }
    }
}