using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS
{
    public class CharacterStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public float maxStamina;
        public float currentStamina;

        [Header("Poise")]
        public float totalPoiseDefence;
        public float currentPoiseDefence;
        public float offensivePoiseBonus;
        public float totalPoiseResetTime = 15;
        public float poiseResetTimer = 0;

        public bool isDead = false;
        private void Start()
        {
            currentPoiseDefence = totalPoiseDefence;
        }
        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }
        public virtual void HandlePoiseResetTimer()
        {
            if(poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else
            {
                currentPoiseDefence = totalPoiseDefence;
            }
        }
        public virtual void TakeDamageNoAnimation(int damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }
    }
}