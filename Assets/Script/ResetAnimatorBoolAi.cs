using UnityEngine;

namespace DS
{
    public class ResetAnimatorBoolAi : ResetAnimatorBool
    {
        [SerializeField] private string _isPhaseShifting = "isPhaseShifting";
        [SerializeField] private bool _isPhaseShiftingStatus = false;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            animator.SetBool(_isPhaseShifting, _isPhaseShiftingStatus);
        }
    }
}