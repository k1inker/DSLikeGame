using UnityEngine;

namespace DS
{
    [CreateAssetMenu(menuName = "Item Actions/Heavy Attack Action")]
    public class HeavyAttackAction : ItemAction
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
                HandleHeavyWeaponCombo(character);
            }
            else
            {
                if (character.isInteracting)
                    return;
                if (character.canDoCombo)
                    return;

                HandleHeavyAttack(character);
                character.characterCombatManager.currentAttackType = AttackType.light;

            }
        }
        private void HandleHeavyAttack(CharacterManager character)
        {
            Debug.Log(character.characterCombatManager.OH_Heavy_Attack_1);
            character.characterAnimatorManager.PlayTargetAnimationWithRootMotion(character.characterCombatManager.OH_Heavy_Attack_1, true);
            character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Heavy_Attack_1;
        }
        private void HandleHeavyWeaponCombo(CharacterManager character)
        {
            if (character.canDoCombo)
            {
                character.animator.SetBool("canDoCombo", false);
                if (character.characterCombatManager.lastAttack == character.characterCombatManager.OH_Heavy_Attack_1)
                {
                    character.characterAnimatorManager.PlayTargetAnimationWithRootMotion(character.characterCombatManager.OH_Heavy_Attack_2, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Heavy_Attack_2;
                }
            }
        }
    }
}
