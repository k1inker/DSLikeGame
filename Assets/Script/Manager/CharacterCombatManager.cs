using DS;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    CharacterManager _character;

    [Header("Attack Type")]
    public AttackType currentAttackType;

    [Header("Attack Animations")]
    public string OH_Light_Attack_1 = "1_Heavy_Light_attack_01";
    public string OH_Light_Attack_2 = "1_Heavy_Light_attack_02";
    public string OH_Heavy_Attack_1 = "1_Heavy_Heavy_attack_01";
    public string OH_Heavy_Attack_2 = "1_Heavy_Heavy_attack_02";

    public string lastAttack;
    protected virtual void Awake()
    {
        _character = GetComponent<CharacterManager>();
    }
    public virtual void SetBlockingAbsorptionsFromBlockingWeapon()
    {
        if(_character.isUsingRightHand)
        {
            _character.characterStatsManager.blockingStabilityRating = _character.characterWeaponSlotManager.rightWeapon.stability;
        }
        else if(_character.isUsingLeftHand)
        {
            _character.characterStatsManager.blockingStabilityRating = _character.characterWeaponSlotManager.leftWeapon.stability;
        }
    }
    public virtual void DrainStaminaBasedOnAttack()
    {
        //if you want AI to lose Stamina during attacks
    }
    public virtual void AttemptBlock(DamageCollider attackingWeapon, float damage,string blockAnimation)
    {
        float staminaDamageAbsorption = (damage * attackingWeapon.guardBreakModifier) * (_character.characterStatsManager.blockingStabilityRating / 100);
        float staminaDamage = damage * attackingWeapon.guardBreakModifier - staminaDamageAbsorption;

        _character.characterStatsManager.DeductStamina(staminaDamage);
        if(_character.characterStatsManager.currentStamina <= 0)
        {
            _character.isBlocking = false;
            _character.characterAnimatorManager.PlayTargetAnimation("Destroy Block Guard", true);
        }
        else
        {
            _character.characterAnimatorManager.PlayTargetAnimation(blockAnimation, true);
        }
    }
}