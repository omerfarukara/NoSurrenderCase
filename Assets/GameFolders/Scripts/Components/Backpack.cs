using System;
using DG.Tweening;
using GameFolders.Scripts.Controllers.AI;
using GameFolders.Scripts.Controllers.Player;
using GameFolders.Scripts.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFolders.Scripts.Components
{
    public class Backpack : MonoBehaviour
    {
        [SerializeField] private Vector2 xRange;
        [SerializeField] private Vector2 zRange;

        private Tween _scaleTween;
        private BoxCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            _scaleTween = transform.DOScale(Vector3.one * 1.25f, 1f).SetLoops(-1, LoopType.Yoyo);
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, 180 * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController player))
            {
                other.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
                GameManager.Instance.Score += 100;
            }
            else if (other.TryGetComponent(out AIController ai))
            {
                other.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
                ai.Score += 100;
            }

            Close();
        }

        private void Close()
        {
            _scaleTween.Kill();
            transform.localScale = Vector3.zero;
            _collider.enabled = false;
            NewPos();
        }

        private void NewPos()
        {
            float x = Random.Range(xRange.x, xRange.y);
            float z = Random.Range(zRange.x, zRange.y);

            transform.localPosition = new Vector3(x, transform.localPosition.y, z);
            Invoke(nameof(Active), 2);
        }

        private void Active()
        {
            _collider.enabled = true;
            transform.localScale = Vector3.one;
            _scaleTween = transform.DOScale(Vector3.one * 1.25f, 1f).SetLoops(-1, LoopType.Yoyo);
        }
    }
}