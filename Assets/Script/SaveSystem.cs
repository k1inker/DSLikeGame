using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
    private static string _pathSettings = Application.persistentDataPath + "/settings.sun";
    public static void SaveSettings(float volume, float sensativity)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(_pathSettings, FileMode.Create))
        {
            Data data = new Data(volume, sensativity);
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
            data = new Data(5, 400);
            SaveSettings(data.volume, data.sensitivity); 
        }
        return data;
    }
}
