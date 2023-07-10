using Game.Scripts.General;
using GameFolders.Scripts.General.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameFolders.Scripts.General
{
    public class DataManager : MonoSingleton<DataManager>
    {
        [SerializeField] internal EventData eventData;
        [SerializeField] internal CharacterMovementData characterMovementData;

    }
}