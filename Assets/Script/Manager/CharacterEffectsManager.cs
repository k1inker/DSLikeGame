using System.Diagnostics;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    [Header("Damage FX")]
    public GameObject bloodSplaterFX;

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
        GameObject blood = Instantiate(bloodSplaterFX, bloodSplatterLocation, Quaternion.identity);
    }
}
