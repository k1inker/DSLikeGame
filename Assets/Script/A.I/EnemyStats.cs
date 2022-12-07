using UnityEngine;

namespace DS
{
    public class EnemyStats : CharacterStats
    {
        private EnemyAnimatorManager _enemyAnimatorManager;
        private void Awake()
        {
            _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        }
        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }
        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (isDead)
                return;

            currentHealth = currentHealth - damage;


            _enemyAnimatorManager.PlayTargetAnimation("Damage",true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                _enemyAnimatorManager.PlayTargetAnimation("Death", true);
                isDead = true;
            }
        }
    }
}
