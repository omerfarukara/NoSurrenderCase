using System;
using System.Collections;
using System.Collections.Generic;
using GameFolders.Scripts.General;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameFolders.Scripts.Controllers.Timer
{
    public class Timer : MonoBehaviour
    {
        #region Properties and Fields Classes

        [SerializeField] private UnityEvent _succesCompletedEvent;
        [SerializeField] private float _duration;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private StringFormat format;
        
        private static EventData EventData => DataManager.Instance.eventData;

        private float _temporaryTimer;
        private float TemporaryTimer
        {
            get => _temporaryTimer;
            set
            {
                _temporaryTimer = value;
                if (!(value < 0)) return;
                IsDone = true;
                _succesCompletedEvent?.Invoke();
            }
        }

        private bool IsDone { get; set; }
        
        #endregion

        private enum StringFormat
        {
            None,
            Time
        }


        private void Awake()
        {
            IsDone = true;
        }

        private void Update()
        {
            if (!IsDone)
            {
                TemporaryTimer -= Time.deltaTime;
                if (_timerText != null)
                {
                    switch (format)
                    {
                        case StringFormat.None:
                            _timerText.text = ((int)TemporaryTimer).ToString();
                            break;
                        case StringFormat.Time:
                            _timerText.text = FormatTime(TemporaryTimer);
                            break;
                    }
                }
            }
        }
        
        string FormatTime(float timeInSeconds)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60f);

            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        public void TimerActive(float delay)
        {
            StartCoroutine(StartTimerActive(delay));
        }
        
        IEnumerator StartTimerActive(float delay)
        {
            TemporaryTimer = _duration;
            if (_timerText != null) _timerText.text = ((int)TemporaryTimer).ToString();
            yield return new WaitForSeconds(delay);
            IsDone = false;
        }

        public void OnPlayEvent()
        {
            gameObject.SetActive(false);
            EventData.OnPlay?.Invoke();
        }

        public void TimeFinished()
        {
            EventData.OnFinishLevel?.Invoke();
        }
    }
}