using UnityEngine;

namespace DS
{
    [CreateAssetMenu(menuName ="Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;

        [Header("Animator Replacer")]
        public AnimatorOverrideController weaponController;
        //public string offHandIdleAnimation;

        [Header("WeaponType")]
        public WeaponType weaponType;

        [Header("Damage")]
        public int baseDamage = 25;
        public int criticalDamageMultiplier = 4;

        [Header("Damage Modifiers")]
        public float lightAttackDamageModifier = 1;
        public float heavyAttackDamageModifier = 2;
        public float guardBreakModifier = 1;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Stability")]
        public int stability = 70;

        [Header("Stamina Costs")]
        public int baseStaminaCost = 25;
        public float lightAttackStaminaMultiplier = 1;
        public float heavyAttackStaminaMultiplier = 2;

        [Header("Item Actions")]
        public ItemAction hold_RB_Action;
        public ItemAction tap_RB_Action;
        public ItemAction hold_LB_Action;
        public ItemAction tap_LB_Action;
    }
}