using System.Collections.Generic;
using UnityEngine;

namespace GameFolders.Scripts.SoundSystem
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "Data/Sound Data")]
    public class SoundData : ScriptableObject
    {
        [SerializeField] private SoundClip[] soundClips;

        public IEnumerable<SoundClip> SoundClips => soundClips;
    }
}
