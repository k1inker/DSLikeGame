using UnityEngine;

namespace DS
{
    [CreateAssetMenu(menuName = "Item Actions/Parry Action")]
    public class ParringAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
                return;

            WeaponItem parryingWeapon = player.playerWeaponSlotManager.currentItemBeingUsed as WeaponItem;

            if(parryingWeapon.weaponType == WeaponType.Shield)
            {
                player.playerAnimatorManager.PlayTargetAnimation("Parry", true);
            }
        }
    }
}