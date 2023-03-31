using UnityEngine;

namespace DS
{
    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
        [Header("Probality weapon")]
        [SerializeField] private WeaponItem _easyWeapon;
        [SerializeField] private WeaponItem _shieldWeapon;
        [SerializeField] private WeaponItem _heavyWeapon;
        [SerializeField] private WeaponItem _katanaWeapon;
        protected override void Awake()
        {
            WeaponType loadWeapon = SaveSystem.LoadWeapon();
            if (loadWeapon == WeaponType.EasySword)
            {
                rightWeapon = _easyWeapon;
                leftWeapon = _shieldWeapon;
            }
            else if (loadWeapon == WeaponType.OneHandHeavySword)
            {
                rightWeapon = _heavyWeapon;
                leftWeapon = null;
            }
            else if (loadWeapon == WeaponType.TwoHandHeavySword)
            {
                rightWeapon = _katanaWeapon;
                leftWeapon = null;
            }
            base.Awake();
        }
    }
}