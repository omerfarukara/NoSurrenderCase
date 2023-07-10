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

        private static EventData EventData => DataManager.Instance.eventData;

        public GameState GameState { get; set; } = GameState.Idle;

        public int Gold
        {
            get => PlayerPrefs.GetInt("Gold");
            set => PlayerPrefs.SetInt("Gold", value);
        }

        public int _score;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                EventData.Score?.Invoke();
            }
        }

        #endregion
   
        #region MonoBehaviour Methods

    
        private void OnEnable()
        {
            EventData.OnFinishLevel += OnFinish;
            EventData.OnLoseLevel += OnLose;
            EventData.OnPlay += OnPlay;
        }

        private void OnPlay()
        {
            GameState = GameState.Play;
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
