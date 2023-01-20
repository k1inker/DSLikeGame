using DS;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    [Header("Attack Type")]
    public AttackType currentAttackType;

    public virtual void DrainStaminaBasedOnAttack()
    {

    }
}
