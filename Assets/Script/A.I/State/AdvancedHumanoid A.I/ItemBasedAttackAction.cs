using UnityEngine;

namespace DS
{
    public class ItemBasedAttackAction : MonoBehaviour
    {
        [Header("Attack Type")]
        public AttackType attackType = AttackType.light;

        [Header("Action Combo Settings")]
        public bool actionCanCombo = false;

        [Header("Wich Hand Action")]
        public bool isRightHandedAction = true;

        [Header("Action Settings")]
        public int attackScore = 3;
        public float recoveryTime = 2;
        public float maximumAttackAngle = 35;
        public float minimumAttackAngle = -35;
        public float minimumDistanceToAttack = 0;
        public float maximumDistanceToAttack = 2;

        public void PerformAttackAction(EnemyManager enemy)
        {
            if(isRightHandedAction)
            {
                enemy.UpdateWichHandCharacterIsUsing(true);
                PerformRightHandItemActionBased(enemy);
            }
            else
            {
                enemy.UpdateWichHandCharacterIsUsing(false);
                PerformLeftHandItemActionBased(enemy);
            }
        }
        private void PerformRightHandItemActionBased(EnemyManager enemy)
        {
            PerformRightHandMeleeAction(enemy);
            //if add range enemy
        }
        private void PerformLeftHandItemActionBased(EnemyManager enemy)
        {

        }
        private void PerformRightHandMeleeAction(EnemyManager enemy)
        {
            if(attackType == AttackType.light)
            {
                enemy.characterWeaponSlotManager.rightWeapon.tap_RB_Action.PerformAction(enemy);
            }
            else if(attackType == AttackType.heavy)
            {
                enemy.characterWeaponSlotManager.rightWeapon.hold_RB_Action.PerformAction(enemy);
            }
        }    
    }
}