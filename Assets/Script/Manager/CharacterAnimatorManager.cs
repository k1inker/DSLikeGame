using UnityEngine;

namespace DS
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        protected CharacterManager _character;
        protected virtual void Awake()
        {
            _character = GetComponent<CharacterManager>();
        }
        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
        {
            _character.animator.applyRootMotion = isInteracting;
            _character.animator.SetBool("canRotate", canRotate);
            _character.animator.SetBool("isInteracting", isInteracting);
            _character.animator.CrossFade(targetAnim, 0.2f);
        }
        public void PlayTargetAnimationWithRootMotion(string targetAnim, bool isInteracting)
        {
            _character.animator.applyRootMotion = isInteracting;
            _character.animator.SetBool("canRotate", false);
            _character.animator.SetBool("isInteracting", isInteracting);
            _character.animator.SetBool("rootPosit", true);
            _character.animator.CrossFade(targetAnim, 0.2f);
        }
        public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
        {
            _character.animator.applyRootMotion = isInteracting;
            _character.animator.SetBool("isRotatingWithRootMotion", true);
            _character.animator.SetBool("isInteracting", isInteracting);
            _character.animator.CrossFade(targetAnim, 0.2f);
        }
        public virtual void CanRotate()
        {
            _character.animator.SetBool("canRotate", true);
        }
        public virtual void StopRotation()
        {
            _character.animator.SetBool("canRotate", false);
        }
        public virtual void EnableCombo()
        {
            _character.animator.SetBool("canDoCombo", true);
        }
        public virtual void DisableCombo()
        {
            _character.animator.SetBool("canDoCombo", false);
        }
        public virtual void EnableInvulnerable()
        {
            _character.animator.SetBool("isInvulnerable", true);
        }
        public virtual void DisableInvulnerable()
        {
            _character.animator.SetBool("isInvulnerable", false);
        }
        public virtual void EnableParrying()
        {
            _character.isParrying = true;
        }
        public virtual void DisableParrying()
        {
            _character.isParrying = false;
        }
    }
}
