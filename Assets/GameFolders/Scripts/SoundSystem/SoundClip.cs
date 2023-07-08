using UnityEngine;

namespace GameFolders.Scripts.SoundSystem
{
    [System.Serializable]
    public class SoundClip
    {
        [SerializeField] private string name;
        [SerializeField] private AudioClip clip;
        [SerializeField] [Range(0, 1)] private float volume;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public AudioClip Clip
        {
            get => clip;
            set => clip = value;
        }

        public float Volume
        {
            get => volume;
            set => volume = Mathf.Clamp(value, 0, 1);
        }
    }
}