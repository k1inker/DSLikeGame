using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class UIBossHealthBar : MonoBehaviour
    {
        public Text bossName;

        private Slider _slider;
        private void Awake()
        {
            _slider = GetComponentInChildren<Slider>();
            bossName = GetComponentInChildren<Text>();
            SetHealthBarInactive();
        }
        public void SetBossName(string name)
        {
            bossName.text = name;
        }
        public void SetHealthBarToActive()
        {
            _slider.gameObject.SetActive(true);
        }
        public void SetHealthBarInactive()
        {
            _slider.gameObject.SetActive(false);
        }
        public void SetBossMaxHealth(int maxHealth)
        {
            _slider.maxValue = maxHealth;
            _slider.value = maxHealth;
        }
        public void SetBossCurrentHealth(int currentHealth)
        {
            _slider.value = currentHealth;
        }
    }
}