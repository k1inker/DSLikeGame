using UnityEngine;

namespace DS
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        [Header("Weapon Slots")]
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        [Header("Weapon Collider")]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;
    }
}