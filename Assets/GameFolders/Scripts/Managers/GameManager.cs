using System.Collections;
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

        [SerializeField] private int levelCount;
        [SerializeField] private int randomLevelLowerLimit;
        [SerializeField] private int startDelay;
        
        private EventData _eventData;

        public GameState GameState { get; set; } = GameState.Play;

        private int Level
        {
            get => PlayerPrefs.GetInt("Level") > levelCount ? Random.Range(randomLevelLowerLimit, levelCount) : PlayerPrefs.GetInt("Level",1);
            set
            {
                PlayerPrefs.SetInt("RealLevel", value);
                PlayerPrefs.SetInt("Level", value);
            } 
        }
    
        public int RealLevel => PlayerPrefs.GetInt("RealLevel", 1);
        public int SceneLevel => SceneManager.GetActiveScene().buildIndex;
        
        public int Gold
        {
            get => PlayerPrefs.GetInt("Gold");
            set => PlayerPrefs.SetInt("Gold", value);
        }

        #endregion
   
        #region MonoBehaviour Methods

        private void Awake()
        {
            Singleton(true);
            _eventData = Resources.Load("EventData") as EventData;
        }
    
        private void OnEnable()
        {
            _eventData.OnFinishLevel += OnFinish;
            _eventData.OnLoseLevel += OnLose;
        }

        private void Start()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                StartCoroutine(LoadSceneAfterDelay(startDelay));
            }
        }

        #endregion

        #region Listening Methods

        private void OnFinish()
        {
            GameState = GameState.Idle;
            Level++;
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
    
        public void NextLevel()
        {
            SceneManager.LoadScene(Level);
        }
        
        public void TryAgain()
        {
            SceneManager.LoadScene(Level);
        }

        private IEnumerator LoadSceneAfterDelay(float delay)
        {
            yield return new WaitForSeconds(startDelay);
            SceneManager.LoadScene(Level);
        }

        #endregion
    }
}
