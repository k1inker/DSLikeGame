using UnityEngine;

namespace DS
{
    public class CharacterStatsManager : MonoBehaviour
    {
        private CharacterManager _characterManager;

        [Header("Team I.D")]
        public int teamIDNumber = 0;

        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public float maxStamina;
        public float currentStamina;

        [Header("Poise")]
        public float totalPoiseDefence;
        public float currentPoiseDefence;
        public float offensivePoiseBonus;
        public float totalPoiseResetTime = 15;
        public float poiseResetTimer = 0;

        [Header("Blocking Absorptions")]
        public float blockingStabilityRating;
        protected virtual void Awake()
        {
            _characterManager = GetComponent<CharacterManager>();
        }
        private void Start()
        {
            currentPoiseDefence = totalPoiseDefence;
        }
        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }
        public virtual void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else
            {
                currentPoiseDefence = totalPoiseDefence;
            }
        }
        public virtual void TakeDamageNoAnimation(int damage)
        {
            if (_characterManager.isDead)
                return;

            currentHealth -= damage;
            _characterManager.characterSFXManager.PlayRandomDamageSFX();
        }
        public virtual void TakeDamage(int damage, string damageAnimation = "Damage")
        {
            if (_characterManager.isDead)
                return;

            currentHealth = currentHealth - damage;
            _characterManager.characterSFXManager.PlayRandomDamageSFX();
        }
        public virtual void DeductStamina(float staminaToDeduct)
        {
            currentStamina = currentStamina - staminaToDeduct;
        }
        protected int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }
        protected float SetMaxStaminaFromHealthLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }
    }
}