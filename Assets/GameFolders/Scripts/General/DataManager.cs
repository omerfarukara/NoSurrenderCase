using Game.Scripts.General;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameFolders.Scripts.General
{
    public class DataManager : MonoSingleton<DataManager>
    {
        [SerializeField] internal EventData eventData;
    }
}