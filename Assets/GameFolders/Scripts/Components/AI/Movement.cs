using System;
using System.Collections.Generic;
using System.Linq;
using GameFolders.Scripts.Controllers;
using GameFolders.Scripts.Controllers.AI;
using GameFolders.Scripts.Controllers.Player;
using GameFolders.Scripts.General;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace GameFolders.Scripts.Components.AI
{
    public class Movement : MonoBehaviour
    {
        #region Properties and Fields Classes

        //private NavMeshAgent _agent;
        [SerializeField] private float forwardSpeed;
        private static EventData EventData => DataManager.Instance.eventData;

        #endregion

        internal bool _isRunning;

        private Transform target;
        private Rigidbody _rigidbody;
        private AIController _previousAi;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            //_agent = GetComponent<NavMeshAgent>();
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
            if (!_isRunning) return;

            //_agent.SetDestination(target.position);
            Vector3 direction = (target.position - transform.position).normalized;

            _rigidbody.MovePosition(transform.position + direction * (Time.deltaTime * forwardSpeed));
            transform.LookAt(target);
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