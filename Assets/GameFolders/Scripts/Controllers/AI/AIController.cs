using System;
using DG.Tweening;
using GameFolders.Scripts.Components.AI;
using GameFolders.Scripts.Controllers.Player;
using GameFolders.Scripts.General;
using GameFolders.Scripts.Helpers;
using UnityEngine;

namespace GameFolders.Scripts.Controllers.AI
{
    public class AIController : MonoBehaviour
    {
        #region Properties and Fields Classes

        [SerializeField] private Animator _animator;

        private Rigidbody _rigidbody;
        private Movement _aiMovement;

        private static EventData EventData => DataManager.Instance.eventData;

        private bool _isDead;
        public bool IsDead
        {
            get => _isDead;
            set
            {
                _isDead = value;
                if (value)
                {
                    EventData.GamerCount?.Invoke();
                    transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
                    {
                        GameController.Instance._ais.Remove(this);
                        Destroy(gameObject);
                    });
                }
            }
        }

        private int _score;

        public int Score
        {
            get => _score;
            set => _score = value;
        }

        #endregion

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _aiMovement = GetComponent<Movement>();
        }

        private void OnEnable()
        {
            EventData.OnPlay += AnimatorRun;
        }

        private void AnimatorRun()
        {
            _animator.SetTrigger(GameConst.Animation.RUN);
        }

        private void AnimatorIdle()
        {
            _animator.SetTrigger(GameConst.Animation.IDLE);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerController player))
            {
                // - Direction yeme sonra yeni target
                _aiMovement._isRunning = false;

                Vector3 direction = collision.contacts[0].point - transform.position;
                direction.Set(direction.x, 0, direction.z);

                player.TakeKnock(direction.normalized, 8);
                TakeKnock(-direction.normalized, -3f);

                _aiMovement.SetTargetProcess();
            }
            else if (collision.gameObject.TryGetComponent(out AIController ai))
            {
                // - Direction yeme sonra yeni target
                _aiMovement._isRunning = false;

                Vector3 direction = collision.contacts[0].point - transform.position;
                direction.Set(direction.x, 0, direction.z);

                ai.TakeKnock(direction.normalized, 8);
                TakeKnock(-direction.normalized, -3f);

                _aiMovement.SetTargetProcess();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ToonWater>())
            {
                IsDead = true;
            }
        }


        private void OnDisable()
        {
            EventData.OnPlay -= AnimatorRun;
        }

        internal void TakeKnock(Vector3 direction, float force)
        {
            AnimatorIdle();
            //Vector3 direction = transform.forward;

            VelocityAndAngularReset();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            _rigidbody.AddForce(direction * force, ForceMode.Impulse);

            ResetProcess();
        }

        private void VelocityAndAngularReset()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        private void ResetProcess()
        {
            Invoke(nameof(AnimatorRun), 1);
            Invoke(nameof(VelocityAndAngularReset), 1);
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }
}