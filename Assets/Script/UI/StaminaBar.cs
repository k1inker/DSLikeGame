using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class StaminaBar : MonoBehaviour
    {
        private Slider _slider;
        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }
        public void SetMaxStamina(float maxStamina)
        {
            _slider.maxValue = maxStamina;
            _slider.value = maxStamina;
        }
        public void SetCurrentStamina(float currentStamina)
        {
            _slider.value = currentStamina;
        }
    }
}
