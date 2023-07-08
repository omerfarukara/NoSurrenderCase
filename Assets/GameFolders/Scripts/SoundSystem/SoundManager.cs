using System.Collections;
using System.Collections.Generic;
using GameFolders.Scripts.General;
using UnityEngine;

namespace GameFolders.Scripts.SoundSystem
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        [SerializeField] private int maxMultipleSound = 5;

        private SoundData _soundData;

        private readonly Dictionary<string, float> _volume = new Dictionary<string, float>();
        private readonly Dictionary<string, AudioClip> _clip = new Dictionary<string, AudioClip>();

        private AudioSource[] _audioSources;

        private void Awake()
        {
            _soundData = Resources.Load("SoundData") as SoundData;
            Singleton(true);
            Initiate();
        }

        private void Initiate()
        {
            foreach (SoundClip soundClip in _soundData.SoundClips)
            {
                _volume.Add(soundClip.Name, soundClip.Volume);
                _clip.Add(soundClip.Name, soundClip.Clip);
            }
        
            for (int i = 0; i < maxMultipleSound; i++)
            {
                GameObject newAudioSource =  new GameObject();
                newAudioSource.AddComponent<AudioSource>();
                newAudioSource.name = $"AudioSource {i}";
                newAudioSource.transform.parent = transform;
            }
        
            _audioSources = GetComponentsInChildren<AudioSource>();
        }

        public void Play(string soundName)
        {
            foreach (AudioSource audioSource in _audioSources)
            {
                if (audioSource.isPlaying) continue;
                
                audioSource.clip = _clip[soundName];
                audioSource.volume = _volume[soundName];
                audioSource.Play();
                
                break;
            }
        }

        public void PlayOnIncrease(string soundName, float coefficient)
        {
            foreach (AudioSource audioSource in _audioSources)
            {
                if (audioSource.isPlaying) continue;
                
                audioSource.clip = _clip[soundName];
                audioSource.volume = _volume[soundName];
                StartCoroutine(IncreaseVolume(audioSource, coefficient));
                
                break;
            }
        }

        public void PlayOnDecrease(string soundName, float coefficient)
        {
            foreach (AudioSource audioSource in _audioSources)
            {
                if (audioSource.isPlaying) continue;
                
                audioSource.clip = _clip[soundName];
                audioSource.volume = _volume[soundName];
                StartCoroutine(DecreaseVolume(audioSource, coefficient));
                
                break;
            }
        }

        private IEnumerator IncreaseVolume(AudioSource audioSource, float coefficient)
        {
            float clipLenght = audioSource.clip.length;
            float currentTime = clipLenght;

            audioSource.Play();

            while (currentTime > 0)
            {
                currentTime -= Time.deltaTime * coefficient;
                audioSource.volume -= currentTime / clipLenght;
                yield return null;
            }

            audioSource.volume = 0;
        }

        private IEnumerator DecreaseVolume(AudioSource audioSource, float coefficient)
        {
            float clipLenght = audioSource.clip.length;
            float currentTime = clipLenght;

            audioSource.Play();

            while (currentTime < 1)
            {
                currentTime += Time.deltaTime * coefficient;
                audioSource.volume += currentTime / clipLenght;
                yield return null;
            }

            audioSource.volume = 1;
        }
    }
}
