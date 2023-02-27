using UnityEngine;

namespace DS
{
    [CreateAssetMenu(menuName ="Item Actions/Blocking Action")]
    public class BlockingAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting)
                return;
            if (character.isBlocking)
                return;

            character.characterAnimatorManager.PlayTargetAnimation("Block Start", false, true);
            character.isBlocking = true;
        }
    }
}