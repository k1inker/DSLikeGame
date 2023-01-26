using UnityEngine;

namespace DS
{
    [CreateAssetMenu(menuName = "Item Actions/Light Attack Action")]
    public class LightAttackAction : ItemAction
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
                HandleLightWeaponCombo(player);
                player.inputHandler.comboFlag = false;
            }
            else
            {
                if (player.isInteracting)
                    return;
                if (player.canDoCombo)
                    return;

                HandleLightAttack(player);
                player.characterCombatManager.currentAttackType = AttackType.light;

            }
        }
        private void HandleLightAttack(PlayerManager player)
        {
            player.playerAnimatorManager.PlayTargetAnimationWithRootMotion(player.playerCombatManager.OH_Light_Attack_1, true);
            player.playerCombatManager.lastAttack = player.playerCombatManager.OH_Light_Attack_1;
        }
        private void HandleLightWeaponCombo(PlayerManager player)
        {
            if (player.inputHandler.comboFlag)
            {
                player.animator.SetBool("canDoCombo", false);
                if (player.playerCombatManager.lastAttack == player.playerCombatManager.OH_Light_Attack_1)
                {
                    player.playerAnimatorManager.PlayTargetAnimationWithRootMotion(player.playerCombatManager.OH_Light_Attack_2, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.OH_Light_Attack_2;
                }
            }
        }
    }
}