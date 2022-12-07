using UnityEngine;

namespace DS
{
    [CreateAssetMenu(menuName = "A.I/Enemy Actions/Attack Action")]
    public class EnemyAttackAction : EnemyAction
    {
        public bool canCombo;

        public EnemyAttackAction comboNextAction;
        public int attackScore = 3;
        public float recoveryTime = 2;

        public float maximumAttackAngle = 35;
        public float minimumAttackAngle = -35;

        public float minimumDistanceToAttack = 0;
        public float maximumDistanceToAttack = 3;
    }
}