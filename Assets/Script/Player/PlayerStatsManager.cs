using System.Threading;
using TMPro;
using UnityEngine;
namespace DS
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        private PlayerManager _player;
       
        private HealthBar _healthBar;
        private StaminaBar _staminaBar;

        private float _staminaRegenAmount = 30;
        private float _staminaRegenTimer = 0;
        protected override void Awake()
        {
            base.Awake();
            _healthBar = FindObjectOfType<HealthBar>();
            _staminaBar = FindObjectOfType<StaminaBar>();

            _player = GetComponent<PlayerManager>();
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
            if (_player.isInvulnerable)
                return;
            if (_player.isDead)
                return;
            base.TakeDamage(damage, damageAnimation = "Damage");
            _healthBar.SetCurrentHealth(currentHealth);
            _player.playerAnimatorManager.PlayTargetAnimationWithRootMotion(damageAnimation, true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                _player.isDead = true;
                _player.playerAnimatorManager.PlayTargetAnimationWithRootMotion("Death", true);
            }
        }
        public override void TakeDamageNoAnimation(int damage)
        {
            base.TakeDamageNoAnimation(damage);
            _healthBar.SetCurrentHealth(currentHealth);
        }
        public override void TakeStaminaDamage(float damage)
        {
            base.TakeStaminaDamage(damage);
            _staminaBar.SetCurrentStamina(currentStamina);   
        }
        public void RegenerateStamina()
        {
            if(_player.isInteracting)
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
            else if(poiseResetTimer <= 0 && !_player.isInteracting)
            {
                currentPoiseDefence = totalPoiseDefence;
            }
        }
    }
}
