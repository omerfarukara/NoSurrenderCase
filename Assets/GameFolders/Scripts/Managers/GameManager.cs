using System.Collections;
using Game.Scripts.General;
using GameFolders.Scripts.General;
using GameFolders.Scripts.General.FGEnum;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace GameFolders.Scripts.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        #region Fields And Properties

        private EventData _eventData;

        public GameState GameState { get; set; } = GameState.Play;

        public int Gold
        {
            get => PlayerPrefs.GetInt("Gold");
            set => PlayerPrefs.SetInt("Gold", value);
        }

        #endregion
   
        #region MonoBehaviour Methods

        private void Awake()
        {
            _eventData = Resources.Load("EventData") as EventData;
        }
    
        private void OnEnable()
        {
            _eventData.OnFinishLevel += OnFinish;
            _eventData.OnLoseLevel += OnLose;
        }

        #endregion

        #region Listening Methods

        private void OnFinish()
        {
            GameState = GameState.Idle;
        }

        private void OnLose()
        {
            GameState = GameState.Lose;
        }

        #endregion
    
        #region Unique Methods

        public bool Playability()
        {
            return GameState == GameState.Play;
        }
    
        #endregion
    }
}
