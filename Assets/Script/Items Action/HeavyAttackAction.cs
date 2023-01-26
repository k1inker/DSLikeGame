using UnityEngine;

namespace DS
{
    [CreateAssetMenu(menuName = "Item Actions/Heavy Attack Action")]
    public class HeavyAttackAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if ((player.characterWeaponSlotManager.rightWeapon.baseStaminaCost * player.characterWeaponSlotManager.rightWeapon.lightAttackStaminaMultiplier)
                > player.playerStatsManager.currentStamina)
                return;

            player.playerEffectsManager.PlayWeaponFX(false);

            if (player.canDoCombo)
            {
                player.inputHandler.comboFlag = true;
                HandleHeavyWeaponCombo(player);
                player.inputHandler.comboFlag = false;
            }
            else
            {
                if (player.isInteracting)
                    return;
                if (player.canDoCombo)
                    return;

                HandleHeavyAttack(player);
                player.characterCombatManager.currentAttackType = AttackType.light;

            }
        }
        private void HandleHeavyAttack(PlayerManager player)
        {
            player.playerAnimatorManager.PlayTargetAnimationWithRootMotion(player.playerCombatManager.OH_Heavy_Attack_1, true);
            player.playerCombatManager.lastAttack = player.playerCombatManager.OH_Heavy_Attack_1;
        }
        private void HandleHeavyWeaponCombo(PlayerManager player)
        {
            if (player.inputHandler.comboFlag)
            {
                player.animator.SetBool("canDoCombo", false);
                if (player.playerCombatManager.lastAttack == player.playerCombatManager.OH_Heavy_Attack_1)
                {
                    player.playerAnimatorManager.PlayTargetAnimationWithRootMotion(player.playerCombatManager.OH_Heavy_Attack_2, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.OH_Heavy_Attack_2;
                }
            }
        }
    }
}
