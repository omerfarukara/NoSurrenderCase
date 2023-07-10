using System;
using System.Collections.Generic;
using System.Linq;
using GameFolders.Scripts.Controllers;
using GameFolders.Scripts.Controllers.AI;
using GameFolders.Scripts.Controllers.Player;
using GameFolders.Scripts.General;
using GameFolders.Scripts.General.Data;
using GameFolders.Scripts.Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace GameFolders.Scripts.Components.AI
{
    public class Movement : MonoBehaviour
    {
        #region Properties and Fields Classes

        private float forwardSpeed;
        private static EventData EventData => DataManager.Instance.eventData;
        private static CharacterMovementData CharacterMovementData => DataManager.Instance.characterMovementData;

        #endregion

        internal bool _isRunning;

        internal Transform target;
        private Rigidbody _rigidbody;
        private AIController _previousAi;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            forwardSpeed = CharacterMovementData.aiForwardSpeed;
        }

        private void OnEnable()
        {
            EventData.OnPlay += SetTarget;
        }

        private void OnDisable()
        {
            EventData.OnPlay -= SetTarget;
        }

        private void Update()
        {
            if(!GameManager.Instance.Playability()) return;

            if (_previousAi  != null && _previousAi.IsDead)
            {
                SetTargetProcess();
            }
            
            if (!_isRunning) return;

            Vector3 direction = (target.position - transform.position).normalized;
            _rigidbody.MovePosition(transform.position + new Vector3(direction.x,0,direction.z) * (Time.deltaTime * forwardSpeed));
            transform.LookAt(new Vector3(target.position.x, 0, target.position.z));

        }

        internal void SetTargetProcess()
        {
            Invoke(nameof(SetTarget), 2);
        }

        private void SetTarget()
        {
            Dictionary<AIController, float> otherAis = new Dictionary<AIController, float>();
            foreach (var ai in GameController.Instance._ais)
            {
                if (transform.position == ai.transform.position ||
                    (_previousAi != null && _previousAi.name == ai.name)) continue;

                otherAis.Add(ai, Mathf.Abs(Vector3.Distance(transform.position, ai.transform.position)));
            }

            if (otherAis.Count > 0)
            {
                KeyValuePair<AIController, float> foundAi = otherAis.OrderBy(x => x.Value).First();

                target = foundAi.Key.transform;
                _previousAi = foundAi.Key;
            }
            else
            {
                target = PlayerController.Instance.transform;
            }

            _isRunning = true;
        }
    }
}