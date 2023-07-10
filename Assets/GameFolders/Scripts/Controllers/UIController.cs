using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using Game.Scripts.Counter;
using Game.Scripts.General;
using GameFolders.Scripts.General;
using GameFolders.Scripts.General.FGEnum;
using GameFolders.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GameFolders.Scripts.Controllers
{
    public class UIController : MonoSingleton<UIController>
    {
        #region Properties and Fields Classes

        [Header("Panels")] [SerializeField] private GameObject victoryPanel;
        [SerializeField] private GameObject losePanel;

        [Header("Buttons")] [SerializeField] Button nextButton;
        [FormerlySerializedAs("beforeGametimer")]
        [FormerlySerializedAs("timer")]
        [Header("Timer")]
        [SerializeField] private Timer.Timer beforeGameTimer;
        [SerializeField] private Timer.Timer inGameTimer;

        [Header("Top Panel")] [SerializeField] private Transform topPanel;
        [SerializeField] private TextMeshProUGUI gamerCountText;
        [SerializeField] private TextMeshProUGUI scoreText;
        

        private int _gamerCount;

        private int GamerCount
        {
            get => _gamerCount;
            set
            {
                _gamerCount = value;
                gamerCountText.text = value.ToString();
            }
        }

        private static EventData EventData => DataManager.Instance.eventData;

        #endregion


        private void OnEnable()
        {
            nextButton.onClick.AddListener(OnNext);

            EventData.OnPlay += OnPlay;
            EventData.OnFinishLevel += OnFinish;
            EventData.OnLoseLevel += OnLose;
            EventData.GamerCount += SetGamerCount;
            EventData.Score += SetScore;
        }

        private void Start()
        {
            EventData.GamerCount?.Invoke();
            EventData.Score?.Invoke();
            
            beforeGameTimer.TimerActive(2);
            inGameTimer.TimerActive(0);
        }

        private void OnDisable()
        {
            EventData.OnFinishLevel -= OnFinish;
            EventData.OnLoseLevel -= OnLose;
            EventData.GamerCount -= SetGamerCount;
            EventData.Score -= SetScore;
            EventData.OnPlay -= OnPlay;

        }

        private void OnPlay()
        {
            topPanel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        }

        private void OnFinish()
        {
            victoryPanel.SetActive(true);
            GameManager.Instance.GameState = GameState.Finish;
        }

        private void OnLose()
        {
        }

        public void OnNext()
        {
            SceneManager.LoadScene(0);
        }

        private void SetGamerCount()
        {
            int index = GameController.Instance._ais.Count(ai => !ai.IsDead);
            GamerCount = index + 1;
        }
        
        private void SetScore()
        {
            //scoreText.text = GameManager.Instance.Score.ToString();
            scoreText.gameObject.GetComponent<NumberCounter>().Value = GameManager.Instance.Score;
        }
    }
}