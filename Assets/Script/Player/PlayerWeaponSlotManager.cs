using UnityEngine;

namespace DS
{
    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
        public override void DrainStaminaLightAttack()
        {
            _character.characterStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }
        public override void DrainStaminaHeavyAttack()
        {
            _character.characterStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }
    }
}