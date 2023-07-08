using System;
using Game.Scripts.General;
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
        [SerializeField] Button tryAgainButton;

        private void Awake()
        {
            _eventData = Resources.Load("EventData") as EventData;
        }

        private void OnEnable()
        {
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
        }

        private void OnLose()
        {
        }

        private void OnTryAgain()
        {
        }
    }
}
