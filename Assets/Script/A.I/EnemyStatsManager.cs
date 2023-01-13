using UnityEngine;

namespace DS
{
    public class EnemyStatsManager : CharacterStatsManager
    {
        [SerializeField] private UIEnemyHealthBar _enemyHealthBar;

        private EnemyAnimatorManager _enemyAnimatorManager;
        public EnemyBossManager enemyBossManager;

        public bool isBoss;
        private void Awake()
        {
            _enemyHealthBar = FindObjectOfType<UIEnemyHealthBar>();

            _enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
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
            base.TakeDamageNoAnimation(damage);

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
                HandleDeath();
            }
        }
        public override void TakeDamage(int damage, string damageAnimation = "Damage")
        {
            if (isDead)
                return;
            base.TakeDamage(damage, damageAnimation = "Damage");

            if (!isBoss)
            {
                _enemyHealthBar.SetHealth(currentHealth);
            }
            else if(isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }

            _enemyAnimatorManager.PlayTargetAnimationWithRootMotion(damageAnimation, true);

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
