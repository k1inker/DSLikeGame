using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Transform lockOnTransform;

    [Header("Movement Flags")]
    public bool isRotatingWithRootMotion;
    public bool canRotate;
}
