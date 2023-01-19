using DS;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Animator animator;
    public CharacterAnimatorManager characterAnimatorManager;
    public CharacterWeaponSlotManager characterWeaponSlotManager;
    public CharacterEffectsManager characterEffectsManager;
    public CharacterStatsManager characterStatsManager;

    public Transform lockOnTransform;

    [Header("Combat Flags")]
    public bool isInvulnerable;
    public bool canDoCombo;
    public bool isUsingRightHand;
    public bool isUsingLeftHand;
    public bool isBlocking;

    [Header("Movement Flags")]
    public bool isRotatingWithRootMotion;
    public bool canRotate;

    [Header("Status")]
    public bool isDead = false;

    public bool isInteracting;
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
    }
}
