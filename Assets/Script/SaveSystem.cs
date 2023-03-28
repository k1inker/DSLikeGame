using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem
{
    private static string _pathSettings = Application.persistentDataPath + "/settings.sun";
    private static string _pathInvetory = Application.persistentDataPath + "/invetory.sun";
    public static void SaveSettings(float effectsVolume,float musicVolume, float sensativity)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(_pathSettings, FileMode.Create))
        {
            Data data = new Data(effectsVolume, sensativity,musicVolume);
            formatter.Serialize(stream, data);
        }
    }
    public static void SaveInvetory(SkinData[] skins)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(_pathInvetory, FileMode.Create))
        {
            foreach(SkinData skin in skins)
                formatter.Serialize(stream, skin);
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
    public static SkinData[] LoadInvetory(SkinChanger data)
    {
        List<SkinData> skins = new List<SkinData>();
        if (File.Exists(_pathInvetory))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(_pathInvetory, FileMode.Open))
            {
                while (stream.Position < stream.Length)
                {
                    skins.Add((SkinData)formatter.Deserialize(stream));
                    Debug.Log(skins.Count);
                }
            }
            return skins.ToArray();
        }
        else
        {
            SaveInvetory(data.FirstLoadInvetory());
            return LoadInvetory(data);
        }
    }
    public static uint LoadModel()
    {
        if (File.Exists(_pathInvetory))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(_pathInvetory, FileMode.Open);
            while (stream.Position < stream.Length)
            {
                var skinData = formatter.Deserialize(stream) as SkinData;
                if (skinData.isChosen)
                {
                    return skinData.index;
                }
            }
            stream.Close();
        }
        return 0;
    }
}
