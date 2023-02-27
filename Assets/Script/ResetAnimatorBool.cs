using UnityEngine;

namespace DS
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        //[SerializeField] private string _isUsingRightHand = "isUsingRightHand";
        //[SerializeField] private bool _isUsingRightHandStatus = false;

        //[SerializeField] private string _isUsingLeftHand = "isUsingLeftHand";
        //[SerializeField] private bool _isUsingLeftHandStatus = false;

        [SerializeField] private string _isInvulnerable = "isInvulnerable";
        [SerializeField] private bool _isInvulnerableStatus = false;

        [SerializeField] private string _isInteractingBool = "isInteracting";
        [SerializeField] private bool _isInteractingStatus = false;

        [SerializeField] private string _canRotateBool = "canRotate";
        [SerializeField] private bool _canRotateStatus = true;

        [SerializeField] private string _isRootPossition = "rootPosit";
        [SerializeField] private bool _isRootPossitionStatus = false;

        [SerializeField] private string _isRootRotation = "isRotatingWithRootMotion";
        [SerializeField] private bool _isRootRotationStatus = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CharacterManager character = animator.GetComponent<CharacterManager>();

            character.isUsingLeftHand = false;
            character.isUsingRightHand = false;
            character.isAttacking = false;
            character.canBeParried = false;
            character.isBlocking = false;

            animator.SetBool(_isInteractingBool, _isInteractingStatus);
            animator.SetBool(_canRotateBool, _canRotateStatus);
            animator.SetBool(_isRootPossition, _isRootPossitionStatus);
            animator.SetBool(_isRootRotation, _isRootRotationStatus);
            animator.SetBool(_isInvulnerable, _isInvulnerableStatus);
        }
    }
}