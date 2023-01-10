using System.Threading;
using TMPro;
using UnityEngine;
namespace DS
{
    public class PlayerStatsManager : CharacterStatsManager
    {

        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private StaminaBar _staminaBar;

        private PlayerAnimatorManager _playerAnimatorManager;
        private PlayerManager _playerManager;

        private float _staminaRegenAmount = 30;
        private float _staminaRegenTimer = 0;
        private void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
            _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        }
        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            _healthBar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromHealthLevel();
            currentStamina = maxStamina;
            _staminaBar.SetMaxStamina(maxStamina);

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
            _playerAnimatorManager.PlayTargetAnimationWithRootMotion("Damage", true);
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                _playerAnimatorManager.PlayTargetAnimationWithRootMotion("Death", true);
                isDead = true;
            }
        }
        public void TakeStaminaDamage(float damage)
        {
            currentStamina = currentStamina - damage;
            _staminaBar.SetCurrentStamina(currentStamina);   
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
                    _staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
        }
        public override void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else if(poiseResetTimer <= 0 && !_playerManager.isInteracting)
            {
                currentPoiseDefence = totalPoiseDefence;
            }
        }
    }
}
