using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound 
{
    public enum SoundType { soundEffect, music}
    public SoundType audioType;

    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    public bool isLoop;
    public bool onAwake;

    [HideInInspector]
    public AudioSource source;
}
