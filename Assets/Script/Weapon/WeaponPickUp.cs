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
            PlayerInvertory playerInvertory;
            PlayerAnimatorManager animatorHandler;

            playerInvertory = playerManager.GetComponent<PlayerInvertory>();
            animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

            animatorHandler.PlayTargetAnimation("Pick Up Item", true, true);
            playerInvertory.LoadWeapon(_weaponRight, _weaponLeft);
            Destroy(gameObject);
        }
    }
}