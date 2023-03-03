using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixerGroup soundMusicMixer;
    public AudioMixerGroup soundEffectsMixer;

    public Sound[] sounds;
    [Header("Hit Sounds")]
    public Sound[] hitSounds;
    [Header("Whooshes Sounds")]
    public Sound[] weaponWhooshesSounds;

    public static AudioManager singelton;
    private void Awake()
    {
        if(singelton == null)
        {
            singelton = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.loop = s.isLoop;

            if(s.audioType == Sound.SoundType.soundEffect)
            {
                s.source.outputAudioMixerGroup = soundEffectsMixer;
            }
            else if(s.audioType == Sound.SoundType.music)
            {
                s.source.outputAudioMixerGroup = soundMusicMixer;
            }

            if(s.onAwake)
            {
                Play(s.name);
            }
        }
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s == null)
            return;
        
        s.source.Play();
    }
}
