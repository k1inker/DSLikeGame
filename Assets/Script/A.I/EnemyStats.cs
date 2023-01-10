using UnityEngine;

namespace DS
{
    public class EnemyStats : CharacterStatsManager
    {
        [SerializeField] private UIEnemyHealthBar _enemyHealthBar;

        private EnemyManager _enemyManager;
        private EnemyAnimatorManager _enemyAnimatorManager;
        public EnemyBossManager enemyBossManager;

        public bool isBoss;
        private void Awake()
        {
            _enemyManager = GetComponent<EnemyManager>();
            _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }
        private void Start()
        {
            if(!isBoss)
                _enemyHealthBar.SetMaxHealth(maxHealth);
        }
        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }
        override public void TakeDamageNoAnimation(int damage)
        {
            currentHealth -= damage;

            if (!isBoss)
            {
                _enemyHealthBar.SetHealth(currentHealth);
            }
            else if(isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }
        public void TakeDamage(int damage, string damageAnimation = "Damage")
        {
            if (isDead)
                return;

            currentHealth = currentHealth - damage;

            if (!isBoss)
            {
                _enemyHealthBar.SetHealth(currentHealth);
            }
            else if(isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }

            _enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            currentHealth = 0;
            _enemyAnimatorManager.PlayTargetAnimation("Death", true);
            isDead = true;
        }
    }
}
