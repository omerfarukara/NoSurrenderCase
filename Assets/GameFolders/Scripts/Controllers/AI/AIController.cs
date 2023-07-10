using System;
using System.Collections;
using DG.Tweening;
using GameFolders.Scripts.Components.AI;
using GameFolders.Scripts.Controllers.Player;
using GameFolders.Scripts.General;
using GameFolders.Scripts.General.Data;
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
        private static CharacterMovementData CharacterMovementData => DataManager.Instance.characterMovementData;


        public bool DoDamage { get; set; } = true;
        
        private bool _isKnocked;

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
                        //Destroy(gameObject);
                        transform.localScale = Vector3.zero;
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
            EventData.OnFinishLevel += AnimatorVictory;
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
                if (!DoDamage)
                {
                    VelocityAndAngularReset();
                }
                else
                {
                    _aiMovement._isRunning = false;
                    _aiMovement.target = null;
                    Vector3 direction = collision.contacts[0].point - transform.position;
                    direction.Set(direction.x, 0, direction.z);
                    player.TakeKnock(direction.normalized, CharacterMovementData.playerTakeKnock);
                    player.DoDamage = false;
                    TakeKnock(-direction.normalized, CharacterMovementData.aiTakeKnockSelf);
                    _aiMovement.SetTargetProcess();
                }
            }
            else if (collision.gameObject.TryGetComponent(out AIController ai))
            {
                if (!DoDamage)
                {
                    VelocityAndAngularReset();
                }
                else
                {
                    _aiMovement._isRunning = false;
                    _aiMovement.target = null;

                    Vector3 direction = collision.contacts[0].point - transform.position;
                    direction.Set(direction.x, 0, direction.z);
                    ai.TakeKnock(direction.normalized, CharacterMovementData.aiTakeKnock);
                    ai.DoDamage = false;
                    TakeKnock(-direction.normalized, CharacterMovementData.aiTakeKnockSelf);
                    _aiMovement.SetTargetProcess();
                }
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
            EventData.OnFinishLevel -= AnimatorVictory;

        }

        private void AnimatorVictory()
        {
            _animator.SetTrigger(GameConst.Animation.VICTORY);
        }

        internal void TakeKnock(Vector3 direction, float force)
        {
            AnimatorIdle();
            VelocityAndAngularReset();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _rigidbody.AddForce(new Vector3(direction.x,0,direction.z) * force, ForceMode.VelocityChange);
            
            ResetProcess();
        }

        private void VelocityAndAngularReset()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            DoDamage = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        private void ResetProcess()
        {
            Invoke(nameof(AnimatorRun), 1);
            Invoke(nameof(VelocityAndAngularReset), 1);
        }
    }
}