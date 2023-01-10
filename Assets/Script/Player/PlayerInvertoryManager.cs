    using UnityEngine;

namespace DS
{
    public class PlayerInvertoryManager : MonoBehaviour
    {
        private PlayerWeaponSlotManager _playerWeaponSlotManager;

        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;

        private void Awake()
        {
            _playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        }
        public void LoadWeapon(WeaponItem weaponItemRight, WeaponItem weaponItemLeft)
        {
            if (weaponItemLeft != null)
            {
                leftWeapon = weaponItemLeft;
                _playerWeaponSlotManager.LoadWeaponOnSlot(weaponItemLeft, true);
            }
            if (weaponItemRight != null)
            {
                rightWeapon = weaponItemRight;
                _playerWeaponSlotManager.LoadWeaponOnSlot(weaponItemRight, false);
            }
        }
    }
}