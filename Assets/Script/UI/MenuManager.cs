using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DS
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Slider _sensitivitySlider;
        [SerializeField] private Slider _effectsVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private CameraHandler _cameraHandler;
        
        //Settings pause
        private float previousTimeScale = 1f;
        public static bool isPaused { get; private set; }
        private void Start()
        {
            Data data = SaveSystem.LoadSettings();
            _sensitivitySlider.value = data.sensitivity;
            _musicVolumeSlider.value = data.musicVolume;
            _effectsVolumeSlider.value = data.effectsVolume;
            SetVolumeMusic();
            SetVolumeEffects();
        }
        public void PauseGame()
        {
            if(Time.timeScale > 0)
            {
                previousTimeScale = Time.timeScale;
                Time.timeScale = 0;
                isPaused = true;
            }
            else if(Time.timeScale == 0)
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
        public void SetSensitivityOnGame()
        {
            _cameraHandler.lookSpeed = _sensitivitySlider.value;
        }
        public void SaveSettings()
        {
            SaveSystem.SaveSettings(_effectsVolumeSlider.value, _musicVolumeSlider.value, _sensitivitySlider.value);
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