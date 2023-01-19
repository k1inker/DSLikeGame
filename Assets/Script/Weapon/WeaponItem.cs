using UnityEngine;

namespace DS
{
    [CreateAssetMenu(menuName ="Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;

        [Header("Animator Replacer")]
        public AnimatorOverrideController weaponController;
        public string offHandIdleAnimation;

        [Header("WeaponType")]
        public WeaponType weaponType;

        [Header("Damage")]
        public int baseDamage = 25;
        public int criticalDamageMultiplier = 4;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Stamina Costs")]
        public int baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;

        [Header("SFX")]
        public AudioClip[] weaponWhooshes;
    }
}