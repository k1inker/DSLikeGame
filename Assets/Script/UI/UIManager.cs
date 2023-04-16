using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private GameObject victoryText;
        [SerializeField] private GameObject lossText;


        private LevelManager _levelManager;

        public Slider attackSlider;
        public Text textWaveIndicator;

        public UIBossHealthBar bossHealthBar;
        private void Awake()
        {
            _levelManager = FindObjectOfType<LevelManager>();
        }
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
        public void StartLevelBtn()
        {
            _levelManager.StartLevel();
        }
        public void ShowResultPanel(bool isLose)
        {
            resultPanel.gameObject.SetActive(true);
            victoryText.gameObject.SetActive(!isLose);
            lossText.gameObject.SetActive(isLose);
        }
    }
}