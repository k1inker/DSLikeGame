using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    [SerializeField] private string _isInteractingBool = "isInteracting";
    [SerializeField] private bool _isInteractingStatus = false;

    [SerializeField] private string _canRotateBool = "canRotate";
    [SerializeField] private bool _canRotateStatus = true;

    [SerializeField] private string _isRootPossition = "rootPosit";
    [SerializeField] private bool _isRootPossitionStatus = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(_isInteractingBool, _isInteractingStatus);
        animator.SetBool(_canRotateBool, _canRotateStatus);
        animator.SetBool(_isRootPossition, _isRootPossitionStatus);
    }
}
