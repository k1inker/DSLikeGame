using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
    private static string _pathSettings = Application.persistentDataPath + "/settings.sun";
    public static void SaveSettings(float effectsVolume,float musicVolume, float sensativity)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(_pathSettings, FileMode.Create))
        {
            Data data = new Data(effectsVolume, sensativity,musicVolume);
            formatter.Serialize(stream, data);
        }
    }
    public static Data LoadSettings()
    {
        Data data = null;
        if (File.Exists(_pathSettings))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(_pathSettings, FileMode.Open);
            
            data = formatter.Deserialize(stream) as Data;
            stream.Close();
        }
        else
        {
            data = new Data(0, 400, 0);
            SaveSettings(data.effectsVolume,data.musicVolume, data.sensitivity); 
        }
        return data;
    }
}
