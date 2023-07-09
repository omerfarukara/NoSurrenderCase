using System;
using Game.Scripts.General;
using GameFolders.Scripts.Components.Player;
using GameFolders.Scripts.Controllers.AI;
using GameFolders.Scripts.General;
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
        
        private static EventData EventData => DataManager.Instance.eventData;

        #endregion

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _movement = GetComponent<Movement>();
        }

        private void OnEnable()
        {
            EventData.OnPlay += AnimatorRun;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out AIController ai))
            {
                // - Direction yeme sonra yeni target
                Vector3 direction = collision.contacts[0].point - transform.position;
                direction.Set(direction.x,0,direction.z);

                
                ai.TakeKnock(direction.normalized,8);
                TakeKnock(-direction.normalized,-3f);
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

        private void OnDisable()
        {
            EventData.OnPlay -= AnimatorRun;
        }

        internal void TakeKnock(Vector3 direction,float force)
        {
            _movement.isKnocked = true;
            AnimatorIdle();
            VelocityAndAngularReset();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            //Vector3 direction = transform.forward;
            _rigidbody.AddForce(direction * force, ForceMode.Impulse);
            
            ResetProcess();
        }

        private void ResetProcess()
        {
            Invoke(nameof(AnimatorRun), 1);
            Invoke(nameof(VelocityAndAngularReset), 1);
            _movement.isKnocked = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        }

        private void VelocityAndAngularReset()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }
}