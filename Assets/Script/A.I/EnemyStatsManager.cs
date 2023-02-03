using UnityEngine;

namespace DS
{
    public class EnemyStatsManager : CharacterStatsManager
    {
        [SerializeField] private UIEnemyHealthBar _enemyHealthBar;

        private EnemyManager _enemy;
        private LevelManager _levelManager;
        protected override void Awake()
        {
            base.Awake();
            _enemyHealthBar = FindObjectOfType<UIEnemyHealthBar>();
            _levelManager = FindObjectOfType<LevelManager>();

            _enemy = GetComponent<EnemyManager>();
        }
        private void Start()
        {
            if(!_enemy.isBoss)
                _enemyHealthBar.SetMaxHealth(maxHealth);

            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;

            maxStamina = SetMaxStaminaFromHealthLevel();
            currentStamina = maxStamina;
        }
        override public void TakeDamageNoAnimation(int damage)
        {
            base.TakeDamageNoAnimation(damage);

            if (!_enemy.isBoss)
            {
                _enemyHealthBar.SetHealth(currentHealth);
            }
            else if(_enemy.isBoss && _enemy.enemyBossManager != null)
            {
                _enemy.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }
            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }
        public override void TakeDamage(int damage, string damageAnimation = "Damage")
        {
            if (_enemy.isDead)
                return;
            base.TakeDamage(damage, damageAnimation = "Damage");

            if (!_enemy.isBoss)
            {
                _enemyHealthBar.SetHealth(currentHealth);
            }
            else if(_enemy.isBoss && _enemy.enemyBossManager != null)
            {
                _enemy.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }

            _enemy.enemyAnimatorManager.PlayTargetAnimationWithRootMotion(damageAnimation, true);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            currentHealth = 0;
            _enemy.enemyAnimatorManager.PlayTargetAnimation("Death", true);
            if (_enemy.isBoss)
            {
                _levelManager.BossHasDefeated();
            }
            else
            {
                _levelManager.DefeatEnemy();
            }

            _enemy.isDead = true;
            Destroy(this.gameObject, 5f);
        }
    }
}
