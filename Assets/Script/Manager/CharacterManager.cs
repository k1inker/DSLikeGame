using DS;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Animator animator;
    public CharacterWeaponSlotManager characterWeaponSlotManager;
    public CharacterAnimatorManager characterAnimatorManager;
    public CharacterEffectsManager characterEffectsManager;
    public CharacterCombatManager characterCombatManager;
    public CharacterStatsManager characterStatsManager;
    public CharacterSFXManager characterSFXManager;

    public Transform lockOnTransform;

    [Header("Combat Flags")]
    public bool isInvulnerable;
    public bool canDoCombo;
    public bool canBeParried;
    public bool isUsingRightHand;
    public bool isUsingLeftHand;
    public bool isBlocking;
    public bool isParrying;
    public bool isAttacking;
    public bool isParied;

    [Header("Movement Flags")]
    public bool isRotatingWithRootMotion;
    public bool canRotate;

    [Header("Status")]
    public bool isDead = false;

    public bool isInteracting;
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        characterSFXManager = GetComponent<CharacterSFXManager>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
        characterCombatManager = GetComponent<CharacterCombatManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
    }
    public virtual void UpdateWichHandCharacterIsUsing(bool usingRightHand)
    {
        if(usingRightHand)
        {
            isUsingLeftHand = false;
            isUsingRightHand = true;
        }
        else
        {
            isUsingRightHand = false;
            isUsingLeftHand = true;
        }
    }
}
