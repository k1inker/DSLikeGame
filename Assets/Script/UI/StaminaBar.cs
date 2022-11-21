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
        public void SetMaxStamina(int maxStamina)
        {
            _slider.maxValue = maxStamina;
            _slider.value = maxStamina;
        }
        public void SetCurrentStamina(int currentStamina)
        {
            _slider.value = currentStamina;
        }
    }
}
