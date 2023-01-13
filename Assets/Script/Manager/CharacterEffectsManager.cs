using System.Diagnostics;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
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
}
