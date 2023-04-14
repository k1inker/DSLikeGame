[System.Serializable]
public class SettingsData
{
    public float effectsVolume;
    public float musicVolume;

    public SettingsData(float volume, float musicVolume)
    {
        this.effectsVolume = volume;
        this.musicVolume = musicVolume;
    }
}
