using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class UIManager : MonoBehaviour
    {
        public Slider attackSlider;
        public Text textWaveIndicator;
        public UIBossHealthBar bossHealthBar;
        public void ShowWaveIndicator(int id)
        {
            textWaveIndicator.gameObject.SetActive(true);
            textWaveIndicator.text = "Wave " + id;
            Invoke(nameof(HideWaveIndicator), 5f);
        }
        private void HideWaveIndicator()
        {
            textWaveIndicator.gameObject.SetActive(false);
        }

    }
}