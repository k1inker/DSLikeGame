using UnityEngine;
using UnityEngine.SceneManagement;

namespace DS
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        private PlayerManager _player;
       
        public HealthBar healthBar;
        public StaminaBar staminaBar;

        private float _staminaRegenAmount = 60;
        private float _staminaRegenTimer = 0;
        protected override void Awake()
        {
            base.Awake();
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();

            _player = GetComponent<PlayerManager>();
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
        public override void TakeDamage(int damage, string damageAnimation = "Damage")
        {
            if (_player.isInvulnerable)
                return;
            if (_player.isDead)
                return;

            base.TakeDamage(damage, damageAnimation);
            healthBar.SetCurrentHealth(currentHealth);
            _player.playerAnimatorManager.PlayTargetAnimationWithRootMotion(damageAnimation, true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                _player.isDead = true;
                _player.playerAnimatorManager.PlayTargetAnimationWithRootMotion("Death", true);
                _player.interAds.ShowAd();
                _player.uiManager.ShowResultPanel(true);
            }
        }
        public void HealHealth(int healCount)
        {
            if(currentHealth + healCount > maxHealth)
                currentHealth = maxHealth;
            else
                currentHealth += healCount;
            healthBar.SetCurrentHealth(currentHealth);
        }
        public override void TakeDamageNoAnimation(int damage)
        {
            base.TakeDamageNoAnimation(damage);
            healthBar.SetCurrentHealth(currentHealth);
        }
        public override void DeductStamina(float damage)
        {
            base.DeductStamina(damage);
            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));   
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
                    if (!_player.isBlocking)
                    {
                        currentStamina += _staminaRegenAmount * Time.deltaTime;
                        staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                    }
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
