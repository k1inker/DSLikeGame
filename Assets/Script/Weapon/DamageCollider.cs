using UnityEngine;

namespace DS
{
    public class DamageCollider : MonoBehaviour
    {
        private Collider damageCollider;

        [Header("Team I.D")]
        public int teamIDNumber = 0;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;


        [Header("Damage")]
        public int currentWeaponDamage = 25;
        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }
        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }
        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }
        private void OnTriggerEnter(Collider collision)
        {
            if(collision.tag == "Character")
            {
                CharacterStatsManager characterStatsManager = collision.GetComponent<CharacterStatsManager>();
                CharacterManager characterManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();
                CharacterEffectsManager playerEffectsManager = collision.GetComponent <CharacterEffectsManager>();

                if (characterStatsManager.teamIDNumber == teamIDNumber)
                    return;

                if(characterManager != null)
                {

                    if(shield != null && characterManager.isBlocking && characterStatsManager != null)
                    {
                        characterStatsManager.TakeDamage(0, "Block Guard");
                        //if(characterStatsManager.currentStamina <= 0)
                        //{
                        //    characterStatsManager.TakeDamage(0, "Destroy Block Guard");
                        //}
                        //characterStatsManager.TakeStaminaDamage(15);
                        return;
                    }
                }

                if(characterStatsManager != null)
                {
                    characterStatsManager.poiseResetTimer = characterStatsManager.totalPoiseResetTime;
                    characterStatsManager.currentPoiseDefence = characterStatsManager.currentPoiseDefence - poiseBreak;

                    Vector3 hitPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    playerEffectsManager.PlayBloodSplatterFX(hitPoint);

                    if (characterStatsManager.currentPoiseDefence > poiseBreak)
                    {
                        characterStatsManager.TakeDamageNoAnimation(currentWeaponDamage);
                    }
                    else
                    {
                        characterStatsManager.TakeDamage(currentWeaponDamage);
                        characterStatsManager.currentPoiseDefence = characterStatsManager.totalPoiseDefence;
                    }
                }
            }
        }
    }
}