using UnityEngine;

public class CharacterManager : MonoBehaviour
{
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

    public bool isInteracting;
}
