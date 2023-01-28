using UnityEngine;

namespace DS
{
    [CreateAssetMenu(menuName = "Item Actions/Parry Action")]
    public class ParringAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting)
                return;

            WeaponItem parryingWeapon = character.characterWeaponSlotManager.currentItemBeingUsed as WeaponItem;

            if(parryingWeapon.weaponType == WeaponType.Shield)
            {
                character.characterAnimatorManager.PlayTargetAnimation("Parry", true);
            }
        }
    }
}