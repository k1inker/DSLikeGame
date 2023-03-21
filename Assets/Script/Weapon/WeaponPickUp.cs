using System;
using UnityEngine;

namespace DS
{
    public class WeaponPickUp : Interactable
    {
        [SerializeField] private WeaponItem _weaponRight;
        [SerializeField] private WeaponItem _weaponLeft;
        private void Start()
        {
            interactableText = _weaponRight.itemName;
        }
        public override void Interact(PlayerManager playerManager)
        {
            PickUpItem(playerManager);
        }
        private void PickUpItem(PlayerManager playerManager)
        {
            playerManager.characterAnimatorManager.PlayTargetAnimationWithRootMotion("Pick Up Item", true);
            playerManager.characterWeaponSlotManager.LoadWeapon(_weaponRight, _weaponLeft);
            Destroy(gameObject);
        }
    }
}