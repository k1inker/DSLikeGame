using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class HealthBar : MonoBehaviour
    {
        private Slider _slider;
        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }
        public void SetMaxHealth(int maxHealth)
        {
            _slider.maxValue = maxHealth;
            _slider.value = maxHealth;
        }
        public void SetCurrentHealth(int currentHealth)
        {
            _slider.value = currentHealth;
        }
    }
}
