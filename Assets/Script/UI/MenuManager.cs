using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Slider _volumeSlider;
    private void Start()
    {
        Data data = SaveSystem.LoadSettings();
        _sensitivitySlider.value = data.sensitivity;
        _volumeSlider.value = data.volume;
        _audioMixer.SetFloat("Volume", data.volume);
    }
    public void SetVolume()
    {
        _audioMixer.SetFloat("Volume", _volumeSlider.value);
    }
    public void ClickSaveSettings()
    {
        SaveSystem.SaveSettings(_volumeSlider.value, _sensitivitySlider.value);
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
