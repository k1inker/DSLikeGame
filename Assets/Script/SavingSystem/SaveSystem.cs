using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace DS
{
    public static class SaveSystem
    {
        private static string _pathSettings = Application.persistentDataPath + "/settings.sun";
        private static string _pathSkin = Application.persistentDataPath + "/skin.sun";
        private static string _pathWeapon = Application.persistentDataPath + "/weapon.sun";
        public static void SaveSettings(float effectsVolume, float musicVolume)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(_pathSettings, FileMode.Create))
            {
                SettingsData data = new SettingsData(effectsVolume, musicVolume);
                formatter.Serialize(stream, data);
            }
        }
        public static void SaveSkin(SkinData[] skins)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(_pathSkin, FileMode.Create))
            {
                foreach (SkinData skin in skins)
                    formatter.Serialize(stream, skin);
            }
        }
        public static void SaveWeapon(WeaponType weaponType)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(_pathWeapon, FileMode.Create))
            {
                formatter.Serialize(stream, weaponType);
            }
        }
        public static SettingsData LoadSettings()
        {
            SettingsData data = null;
            if (File.Exists(_pathSettings))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(_pathSettings, FileMode.Open);

                data = (SettingsData)formatter.Deserialize(stream);
                stream.Close();
            }
            else
            {
                data = new SettingsData(0, 0);
                SaveSettings(data.effectsVolume, data.musicVolume);
            }
            return data;
        }
        public static SkinData[] LoadSkin()
        {
            List<SkinData> skins = new List<SkinData>();
            if (File.Exists(_pathSkin))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(_pathSkin, FileMode.Open))
                {
                    while (stream.Position < stream.Length)
                    {
                        skins.Add((SkinData)formatter.Deserialize(stream));
                    }
                }
                return skins.ToArray();
            }
            else
            {
                SaveSkin(SkinChanger.FirstLoadInvetory());
                return LoadSkin();
            }
        }
        public static WeaponType LoadWeapon()
        {
            WeaponType type = WeaponType.EasySword;
            if (File.Exists(_pathWeapon))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(_pathWeapon, FileMode.Open);

                type = (WeaponType)formatter.Deserialize(stream);
                stream.Close();
                return type;
            }
            else
            {
                SaveWeapon(WeaponType.EasySword);
                return WeaponType.EasySword;
            }
        }
    }
}
