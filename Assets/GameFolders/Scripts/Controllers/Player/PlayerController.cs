using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.General;
using GameFolders.Scripts.Components.Player;
using GameFolders.Scripts.Controllers.AI;
using GameFolders.Scripts.General;
using GameFolders.Scripts.General.Data;
using GameFolders.Scripts.Helpers;
using UnityEngine;

namespace GameFolders.Scripts.Controllers.Player
{
    public class PlayerController : MonoSingleton<PlayerController>
    {
        #region Properties and Fields Classes

        [SerializeField] private Animator _animator;

        private Rigidbody _rigidbody;

        private Movement _movement;

        public bool DoDamage { get; set; } = true;

        private static EventData EventData => DataManager.Instance.eventData;
        private static CharacterMovementData CharacterMovementData => DataManager.Instance.characterMovementData;

        #endregion

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _movement = GetComponent<Movement>();
        }

        private void OnEnable()
        {
            EventData.OnPlay += AnimatorRun;
            EventData.OnFinishLevel += AnimatorVictory;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out AIController ai))
            {
                if (!DoDamage)
                {
                    VelocityAndAngularReset();
                }
                else
                {
                    Vector3 direction = collision.contacts[0].point - transform.position;
                    direction.Set(direction.x, 0, direction.z);

                    ai.TakeKnock(direction.normalized, CharacterMovementData.aiTakeKnock);
                    ai.DoDamage = false;
                    _movement.isKnocked = true;
                    TakeKnock(-direction.normalized, CharacterMovementData.playerTakeKnockSelf); 
                }
            }
        }

        private void AnimatorRun()
        {
            _animator.SetTrigger(GameConst.Animation.RUN);
        }

        private void AnimatorIdle()
        {
            _animator.SetTrigger(GameConst.Animation.IDLE);
        }
        
        private void AnimatorVictory()
        {
            _animator.SetTrigger(GameConst.Animation.VICTORY);
        }

        private void OnDisable()
        {
            EventData.OnPlay -= AnimatorRun;
            EventData.OnFinishLevel -= AnimatorVictory;
        }

        internal void TakeKnock(Vector3 direction, float force)
        {
            AnimatorIdle();
            //_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _rigidbody.AddForce(new Vector3(direction.x,0,direction.z) * force, ForceMode.VelocityChange);

            ResetProcess();
        }

        private void ResetProcess()
        {
            Invoke(nameof(AnimatorRun), 1);
            Invoke(nameof(VelocityAndAngularReset), 1);
            //_rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }


        private void VelocityAndAngularReset()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _movement.isKnocked = false;
            DoDamage = true;
        }
    }
}