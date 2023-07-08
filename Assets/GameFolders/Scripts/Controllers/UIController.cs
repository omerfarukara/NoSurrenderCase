using System;
using GameFolders.Scripts.General;
using GameFolders.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace GameFolders.Scripts.Controllers
{
    public class UIController : MonoSingleton<UIController>
    {
        private EventData _eventData;

        [Header("Panels")]
        [SerializeField] private GameObject victoryPanel;
        [SerializeField] private GameObject losePanel;
    
        [Header("Buttons")]
        [SerializeField] Button nextLevelButton;
        [SerializeField] Button tryAgainButton;

        private void Awake()
        {
            Singleton();
            _eventData = Resources.Load("EventData") as EventData;
        }

        private void OnEnable()
        {
            nextLevelButton.onClick.AddListener(OnNextLevel);
            tryAgainButton.onClick.AddListener(OnTryAgain);
            _eventData.OnFinishLevel += OnFinish;
            _eventData.OnLoseLevel += OnLose;
        }

        private void OnDisable()
        {
            _eventData.OnFinishLevel -= OnFinish;
            _eventData.OnLoseLevel -= OnLose;
        }

        private void OnFinish()
        {
            victoryPanel.SetActive(true);
        }

        private void OnLose()
        {
            losePanel.SetActive(true);
        }

        private void OnNextLevel()
        {
            GameManager.Instance.NextLevel();
        }

        private void OnTryAgain()
        {
            GameManager.Instance.TryAgain();
        }
    }
}
