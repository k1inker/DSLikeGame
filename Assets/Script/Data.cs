[System.Serializable]
public class Data
{
    public float effectsVolume;
    public float musicVolume;
    public float sensitivity;

    public Data(float volume, float sensitivity, float musicVolume)
    {
        this.effectsVolume = volume;
        this.sensitivity = sensitivity;
        this.musicVolume = musicVolume;
    }
}
