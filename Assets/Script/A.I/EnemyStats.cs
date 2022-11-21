
using UnityEngine;

namespace DS
{
    public class EnemyStats : CharacterStats
    {
        private Animator _anim;
        private void Awake()
        {
            _anim = GetComponentInChildren<Animator>();
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

        public void TakeDamege(int damage)
        {
            currentHealth = currentHealth - damage;


            _anim.Play("Damage");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                _anim.Play("Death");
            }
        }
    }
}
