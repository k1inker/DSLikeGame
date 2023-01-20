using System.Collections.Generic;
using UnityEngine;
namespace DS
{
    public class CharacterSFXManager : MonoBehaviour
    {
        private CharacterManager _character;
        private AudioSource _audioSource;

        [Header("Taking Damage Sounds")]
        public AudioClip[] takingDamageSounds;
        private List<AudioClip> _potentialDamageSound;
        private AudioClip _lastDamageSoundPlayed;

        [Header("Weapon Whooshes")]
        private List<AudioClip> _potentialWeaponWhooshes;
        private AudioClip _lastWeaponWhooshes;

        protected virtual void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _character = GetComponent<CharacterManager>();
        }
        public virtual void PlayRandomDamageSFX()
        {
            _potentialDamageSound = new List<AudioClip>();

            foreach(var damageSound in takingDamageSounds)
            {
                if(damageSound != _lastDamageSoundPlayed)
                {
                    _potentialDamageSound.Add(damageSound);
                }
            }

            int randomvalue = Random.Range(0, _potentialDamageSound.Count);
            _lastDamageSoundPlayed = takingDamageSounds[randomvalue];
            _audioSource.PlayOneShot(takingDamageSounds[randomvalue], 0.4f);
        }
        public virtual void PlayRandomWeaponWhoosh()
        {
            _potentialWeaponWhooshes = new List<AudioClip>();

            if(_character.isUsingRightHand)
            {
                foreach (var whooshesSound in _character.characterWeaponSlotManager.rightWeapon.weaponWhooshes)
                { 
                    if (whooshesSound != _lastWeaponWhooshes)
                    {
                        _potentialWeaponWhooshes.Add(whooshesSound);
                    }
                }

                int randomvalue = Random.Range(0, _potentialWeaponWhooshes.Count);
                _lastWeaponWhooshes = _character.characterWeaponSlotManager.rightWeapon.weaponWhooshes[randomvalue];
                _audioSource.PlayOneShot(_character.characterWeaponSlotManager.rightWeapon.weaponWhooshes[randomvalue], 0.4f);
            }
            else
            {
                foreach (var whooshesSound in _character.characterWeaponSlotManager.leftWeapon.weaponWhooshes)
                {
                    if (whooshesSound != _lastWeaponWhooshes)
                    {
                        _potentialWeaponWhooshes.Add(whooshesSound);
                    }
                }

                int randomvalue = Random.Range(0, _potentialWeaponWhooshes.Count);
                _lastWeaponWhooshes = _character.characterWeaponSlotManager.leftWeapon.weaponWhooshes[randomvalue];
                _audioSource.PlayOneShot(_character.characterWeaponSlotManager.leftWeapon.weaponWhooshes[randomvalue], 0.4f);
            }
        }
    }
}