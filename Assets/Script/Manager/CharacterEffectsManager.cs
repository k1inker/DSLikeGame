using System.Diagnostics;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    [Header("Damage FX")]
    public GameObject bloodSplaterFX;

    [Header("Dust FX")]
    public GameObject sandDustFX;

    [Header("WeaponFX")]
    public WeaponFX rightWeaponFX;
    public WeaponFX leftWeaponFX;
    public virtual void PlayWeaponFX(bool isLeft)
    {
        if(!isLeft)
        {
            if(rightWeaponFX != null)
            {
                rightWeaponFX.PlayWeaponFX();
            }
        }
        else
        {
            if(leftWeaponFX != null)
            {
                leftWeaponFX.PlayWeaponFX();
            }
        }
    }
    public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
    {
        Instantiate(bloodSplaterFX, bloodSplatterLocation, Quaternion.identity);
    }
    public virtual void PlaySandDustFX(Vector3 sandDustLocation)
    {
        Instantiate(sandDustFX, sandDustLocation, Quaternion.LookRotation(Vector3.up));
    }
}
