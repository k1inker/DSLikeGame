using System.Runtime.CompilerServices;
using UnityEngine;

namespace DS
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator animator;
        protected CharacterManager _characterManager;
        protected CharacterStatsManager _characterStatsManager;
        public bool canRotate = true;
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            _characterManager = GetComponent<CharacterManager>();
            _characterStatsManager = GetComponent<CharacterStatsManager>();
        }
        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("canRotate", canRotate);
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
        public virtual void CanRotate()
        {
            animator.SetBool("canRotate", true);
        }
        public virtual void StopRotation()
        {
            animator.SetBool("canRotate", false);
        }
        public virtual void EnableCombo()
        {
            animator.SetBool("canDoCombo", true);
        }
        public virtual void DisableCombo()
        {
            animator.SetBool("canDoCombo", false);
        }
        public virtual void EnableInvulnerable()
        {
            animator.SetBool("isInvulnerable", true);
        }
        public virtual void DisableInvulnerable()
        {
            animator.SetBool("isInvulnerable", false);
        }
    }
}
