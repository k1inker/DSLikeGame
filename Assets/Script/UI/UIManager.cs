using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class UIManager : MonoBehaviour
    {
        public Slider attackSlider;

        public Text textWaveIndicator;

        public UIBossHealthBar bossHealthBar;

        private LevelManager _levelManager;

        [Header("Interactable UI")]
        [SerializeField] private Text _interactableText;
        [SerializeField] private GameObject _interactable;
        private Interactable _interactableObject = null;
        private void Awake()
        {
            _levelManager = FindObjectOfType<LevelManager>();
        }
        public void ShowInteractMessageObject(Interactable interactableObject)
        {
            _interactableObject = interactableObject;
            string interactableText = interactableObject.interactableText;
            _interactableText.text = interactableText;
            _interactable.SetActive(true);
        }
        public void HideInteractMessageObject()
        {
            _interactable.SetActive(false);
        }
        public void InteractObject()
        {
            _interactableObject.Interact(GetComponentInParent<PlayerManager>());
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
    }
}