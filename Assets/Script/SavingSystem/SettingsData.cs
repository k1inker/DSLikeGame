[System.Serializable]
public class SettingsData
{
    public float effectsVolume;
    public float musicVolume;
    public float sensitivity;

    public SettingsData(float volume, float sensitivity, float musicVolume)
    {
        this.effectsVolume = volume;
        this.sensitivity = sensitivity;
        this.musicVolume = musicVolume;
    }
}
