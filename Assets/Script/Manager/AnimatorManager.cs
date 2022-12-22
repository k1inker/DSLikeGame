using UnityEngine;

namespace DS
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator anim;

        public bool canRotate = true;
        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("canRotate", false);
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }
        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool useAnimPosition)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("canRotate", false);
            anim.SetBool("isInteracting", isInteracting);
            anim.SetBool("rootPosit", useAnimPosition);
            anim.CrossFade(targetAnim, 0.2f);
        }
    }
}
