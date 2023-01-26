using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace DS
{
    [CreateAssetMenu(menuName ="Item Actions/Blocking Action")]
    public class BlockingAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
                return;
            if (player.isBlocking)
                return;

            player.playerAnimatorManager.PlayTargetAnimation("Block Start", false, true);
            player.isBlocking = true;
        }
    }
}