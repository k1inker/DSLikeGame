using UnityEngine;

namespace DS
{
    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
        public override void DrainStaminaLightAttack()
        {
            _characterStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }
        public override void DrainStaminaHeavyAttack()
        {
            _characterStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }
    }
}