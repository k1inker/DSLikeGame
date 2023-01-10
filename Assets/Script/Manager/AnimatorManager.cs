using UnityEngine;

namespace DS
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator animator;

        public bool canRotate = true;
        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("canRotate", false);
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0.2f);
        }
        public void PlayTargetAnimationWithRootMotion(string targetAnim, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("canRotate", false);
            animator.SetBool("isInteracting", isInteracting);
            animator.SetBool("rootPosit", true);
            animator.CrossFade(targetAnim, 0.2f);
        }
        public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("isRotatingWithRootMotion", true);
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0.2f);
        }
    }
}
