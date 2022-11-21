using UnityEngine;

namespace DS
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator anim;
        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }
        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool useAnimPosition)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.SetBool("useAnimPosit", useAnimPosition);
            anim.CrossFade(targetAnim, 0.2f);
        }
    }
}
