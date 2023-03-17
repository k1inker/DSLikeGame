using UnityEngine;

namespace DS
{
    [CreateAssetMenu(menuName = "Item Actions/Parry Action")]
    public class ParringAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting)
                return;

            character.characterAnimatorManager.PlayTargetAnimationWithRootMotion("Parry", true);
        }
    }
}