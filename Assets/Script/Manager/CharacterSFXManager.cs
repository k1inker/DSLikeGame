using System.Collections.Generic;
using UnityEngine;
namespace DS
{
    public class CharacterSFXManager : MonoBehaviour
    {
        private CharacterManager _character;
        private AudioSource _audioSource;
        private AudioManager _audioManager;

        [Header("Taking Damage Sounds")]
        private Sound[] _takingDamageSounds;
        private List<Sound> _potentialDamageSound;
        private Sound _lastDamageSoundPlayed;

        [Header("Weapon Whooshes")]
        private Sound[] _whooshesSounds;
        private List<Sound> _potentialWeaponWhooshes;
        private Sound _lastWeaponWhooshes;

        protected virtual void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _character = GetComponent<CharacterManager>();

            _audioManager = FindObjectOfType<AudioManager>();
        }
        private void Start()
        {
            _takingDamageSounds = _audioManager.hitSounds;
            _whooshesSounds = _audioManager.weaponWhooshesSounds;
        }
        public virtual void PlayRandomDamageSFX()
        {
            _potentialDamageSound = new List<Sound>();

            foreach(var damageSound in _takingDamageSounds)
            {
                if(damageSound != _lastDamageSoundPlayed)
                {
                    _potentialDamageSound.Add(damageSound);
                }
            }

            int randomvalue = Random.Range(0, _potentialDamageSound.Count);
            _lastDamageSoundPlayed = _takingDamageSounds[randomvalue];

            _audioSource.clip = _lastDamageSoundPlayed.clip;
            _audioSource.volume = _lastDamageSoundPlayed.volume;
            _audioSource.outputAudioMixerGroup = _audioManager.soundEffectsMixer;

            _audioSource.Play();
        }
        public virtual void PlayRandomWeaponWhoosh()
        {
            _potentialWeaponWhooshes = new List<Sound>();
            Debug.Log(_audioManager);
            if (_character.isUsingRightHand)
            {
                foreach (var whooshesSound in _whooshesSounds)
                { 
                    if (whooshesSound != _lastWeaponWhooshes)
                    {
                        _potentialWeaponWhooshes.Add(whooshesSound);
                    }
                }

                int randomvalue = Random.Range(0, _potentialWeaponWhooshes.Count);
                _lastWeaponWhooshes = _whooshesSounds[randomvalue];

                _audioSource.clip = _lastWeaponWhooshes.clip;
                _audioSource.volume = _lastWeaponWhooshes.volume;
                _audioSource.outputAudioMixerGroup = _audioManager.soundEffectsMixer;
                //Debug.Log(_audioManager);
                _audioSource.Play();
            }
        }
    }
}