using UnityEngine;

namespace DS
{
    [CreateAssetMenu(menuName = "Item Actions/Light Attack Action")]
    public class LightAttackAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if ((character.characterWeaponSlotManager.rightWeapon.baseStaminaCost * character.characterWeaponSlotManager.rightWeapon.lightAttackStaminaMultiplier) 
                > character.characterStatsManager.currentStamina)
                return;

            character.isAttacking = true;
            character.characterEffectsManager.PlayWeaponFX(false);

            if (character.canDoCombo)
            {
                HandleLightWeaponCombo(character);
            }
            else
            {
                if (character.isInteracting)
                    return;
                if (character.canDoCombo)
                    return;

                HandleLightAttack(character);
                character.characterCombatManager.currentAttackType = AttackType.light;

            }
        }
        private void HandleLightAttack(CharacterManager character)
        {
            character.characterAnimatorManager.PlayTargetAnimationWithRootMotion(character.characterCombatManager.OH_Light_Attack_1, true);
            character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Light_Attack_1;
        }
        private void HandleLightWeaponCombo(CharacterManager character)
        {
            if (character.canDoCombo)
            {
                character.animator.SetBool("canDoCombo", false);
                if (character.characterCombatManager.lastAttack == character.characterCombatManager.OH_Light_Attack_1)
                {
                    character.characterAnimatorManager.PlayTargetAnimationWithRootMotion(character.characterCombatManager.OH_Light_Attack_2, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Light_Attack_2;
                }
            }
        }
    }
}