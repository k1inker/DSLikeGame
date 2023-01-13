using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Transform lockOnTransform;

    [Header("Combat Flags")]
    public bool isInvulnerable;
    public bool canDoCombo;
    public bool isUsingRightHand;
    public bool isUsingLeftHand;

    [Header("Movement Flags")]
    public bool isRotatingWithRootMotion;
    public bool canRotate;

    public bool isInteracting;
}
