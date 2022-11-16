using TMPro;
using UnityEngine;
namespace SG
{
    public class PlayerStats: CharacterStats
    {

        public HealthBar healthBar;
        public StaminaBar staminaBar;
        private AnimatorHandler _anim;


        private PlayerManager _playerManager;
        private void Awake()
        {
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            _playerManager = GetComponent<PlayerManager>();
            _anim = GetComponentInChildren<AnimatorHandler>();
        }
        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromHealthLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
        }
        private int SetMaxStaminaFromHealthLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }
        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamege(int damage)
        {
            currentHealth = currentHealth - damage;

            healthBar.SetCurrentHealth(currentHealth);
            _anim.PlayTargetAnimation("Damage", true, true);
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                _anim.PlayTargetAnimation("Death", true, true);
            }
        }
        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;
            staminaBar.SetCurrentStamina(currentStamina);   
        }
    }
}
