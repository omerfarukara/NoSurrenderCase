using System;
using GameFolders.Scripts.Controllers;
using GameFolders.Scripts.Managers;
using UnityEngine;

namespace GameFolders.Scripts.Components.Player
{
    public class Movement : MonoBehaviour
    {
        #region Properties and Fields Classes

        //Forward Movement
        [Header("Forward Movement")]
        [SerializeField] private float forwardSpeed;

        //Rotate Movement
        [Header("Rotate Movement")]
        [SerializeField] private float turnSpeed;
    
        //Classes
        private Rigidbody _rigidbody;

        //Fields
        private float horizontal;
        private float vertical;
        internal bool isKnocked;

        #endregion

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }


        private void Update()
        {
            if(!GameManager.Instance.Playability()) return;
            
            horizontal = InputController.Instance.JoystickHorizontal();
            vertical = InputController.Instance.JoystickVertical();
        }

        void FixedUpdate()
        {
            if(!GameManager.Instance.Playability()) return;
            if (isKnocked) return;

            _rigidbody.velocity = transform.forward * (forwardSpeed * Time.deltaTime);

            if (!Input.GetMouseButton(0)) return;
            Vector3 direction = Vector3.right * horizontal + Vector3.forward * vertical;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.fixedDeltaTime);
        }
    }
}