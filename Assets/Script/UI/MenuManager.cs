using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DS
{
    public class MenuManager : MonoBehaviour
    {
        [Header("Canvas object")]
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Slider _effectsVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private CameraHandler _cameraHandler;
        
        //Settings pause
        private float previousTimeScale = 1f;
        public static bool isPaused { get; private set; } = false;
        private void Start()
        {
            SettingsData data = SaveSystem.LoadSettings();
            _musicVolumeSlider.value = data.musicVolume;
            _effectsVolumeSlider.value = data.effectsVolume;
            SetVolumeMusic();
            SetVolumeEffects();
        }
        public void PauseGame()
        {
            if(!isPaused)
            {
                previousTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                isPaused = true;
            }
            else
            {
                Time.timeScale = previousTimeScale;
                isPaused = false;
            }
        }
        public void SetVolumeMusic()
        {
            _audioMixer.SetFloat("Music", Mathf.Log10(_musicVolumeSlider.value)*20);
        }
        public void SetVolumeEffects()
        {
            _audioMixer.SetFloat("Effects", Mathf.Log10(_effectsVolumeSlider.value) * 20);
        }
        public void SaveSettings()
        {
            SaveSystem.SaveSettings(_effectsVolumeSlider.value, _musicVolumeSlider.value);
        }
        public void ChangeScene(int idScene)
        {
            SceneManager.LoadScene(idScene);
        }
        public void ExitAplication()
        {
            Application.Quit();
        }
    }
}