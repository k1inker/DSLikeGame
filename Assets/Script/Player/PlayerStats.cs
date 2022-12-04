using System.Threading;
using TMPro;
using UnityEngine;
namespace DS
{
    public class PlayerStats: CharacterStats
    {

        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private StaminaBar staminaBar;
        private PlayerAnimatorManager _anim;

        private float _staminaRegenAmount = 30;
        private float _staminaRegenTimer = 0;
        private PlayerManager _playerManager;
        private void Awake()
        {
            _healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            _playerManager = GetComponent<PlayerManager>();
            _anim = GetComponentInChildren<PlayerAnimatorManager>();
        }
        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            _healthBar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromHealthLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
        }
        private float SetMaxStaminaFromHealthLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }
        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (_playerManager.isInvulnerable)
                return;
            if(isDead)
                return;
            currentHealth = currentHealth - damage;

            _healthBar.SetCurrentHealth(currentHealth);
            _anim.PlayTargetAnimation("Damage", true, true);
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                _anim.PlayTargetAnimation("Death", true, true);
                isDead = true;
            }
        }
        public void TakeStaminaDamage(float damage)
        {
            currentStamina = currentStamina - damage;
            staminaBar.SetCurrentStamina(currentStamina);   
        }
        public void RegenerateStamina()
        {
            if(_playerManager.isInteracting)
            {
                _staminaRegenTimer = 0;
            }
            else
            {
                _staminaRegenTimer += Time.deltaTime;
                if (currentStamina < maxStamina && _staminaRegenTimer > 1f)
                {
                    currentStamina += _staminaRegenAmount * Time.deltaTime;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
        }
    }
}
