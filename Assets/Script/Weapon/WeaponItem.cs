using UnityEngine;

namespace SG
{
    [CreateAssetMenu(menuName ="Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        [Header("Attack Animations")]
        public string OH_Light_Attack_1;
        public string OH_Light_Attack_2;
        public string OH_Heavy_Attack_1;
        [Header("Stamina Costs")]
        public int baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;
    }
}