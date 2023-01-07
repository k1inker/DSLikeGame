using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Transform lockOnTransform;

    [Header("Combat Flags")]
    public bool isInvulnerable;

    [Header("Movement Flags")]
    public bool isRotatingWithRootMotion;
    public bool canRotate;
}
