using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
namespace DS
{
    public class UIEnemyHealthBar : MonoBehaviour
    {
        private Slider _slider;
        private float _timeUntilBarisHidden = 0;

        private void Awake()
        {
            _slider = GetComponentInChildren<Slider>();
        }
        public void SetHealth(int health)
        {
            _slider.value = health;
            _timeUntilBarisHidden = 3;
        }
        public void SetMaxHealth(int maxHealth)
        {
            _slider.maxValue = maxHealth;
            _slider.value = maxHealth;
        }
        private void Update()
        {
            _timeUntilBarisHidden -= Time.deltaTime;

            if (_slider != null)
            {
                if( _timeUntilBarisHidden <= 0 )
                {
                    _timeUntilBarisHidden = 0;
                    _slider.gameObject.SetActive(false);
                }
                else
                {
                    if (!_slider.gameObject.activeInHierarchy)
                    {
                        _slider.gameObject.SetActive(true);
                    }
                }

                if(_slider.value <= 0)
                {
                    Destroy(_slider.gameObject);
                }
            }
        }
    }
}