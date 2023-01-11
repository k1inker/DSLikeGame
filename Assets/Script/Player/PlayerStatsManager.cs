using System.Threading;
using TMPro;
using UnityEngine;
namespace DS
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        private HealthBar _healthBar;
        private StaminaBar _staminaBar;

        private PlayerAnimatorManager _playerAnimatorManager;
        private PlayerManager _playerManager;

        private float _staminaRegenAmount = 30;
        private float _staminaRegenTimer = 0;
        private void Awake()
        {
            _healthBar = FindObjectOfType<HealthBar>();
            _staminaBar = FindObjectOfType<StaminaBar>();

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
        public override void TakeDamage(int damage, string damageAnimation = "Damage")
        {
            if (_playerManager.isInvulnerable)
                return;
            if (isDead)
                return;
            base.TakeDamage(damage, damageAnimation = "Damage");
            _healthBar.SetCurrentHealth(currentHealth);
            _playerAnimatorManager.PlayTargetAnimationWithRootMotion(damageAnimation, true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                _playerAnimatorManager.PlayTargetAnimationWithRootMotion("Death", true);
            }
        }
        public override void TakeDamageNoAnimation(int damage)
        {
            base.TakeDamageNoAnimation(damage);
            _healthBar.SetCurrentHealth(currentHealth);
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
