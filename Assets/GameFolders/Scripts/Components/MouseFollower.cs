using GameFolders.Scripts.General;
using UnityEngine;

namespace GameFolders.Scripts.Components
{
    public class MouseFollower : MonoBehaviour
    {
        [SerializeField] private float maxScale;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float minSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private LayerMask groundLayer;

        private Camera _camera;
        private EventData _eventData;
        
        private Vector3 _screenPosition;
        private Vector3 _worldPosition;

        private float _defaultScale;
        private float _currentScale;
        private float _currentSpeed;
        private bool _trueTap;

        private void Awake()
        {
            _camera = Camera.main;
            _eventData = Resources.Load("EventData") as EventData;
        }

        void Start()
        {
            _currentSpeed = maxSpeed;
            _currentScale = 0;
            transform.localScale = Vector3.zero;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _screenPosition = Input.mousePosition;

                Ray ray = _camera.ScreenPointToRay(_screenPosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100, groundLayer))
                {
                    if (hit.collider.CompareTag("Ground"))
                    {
                        _trueTap = true;
                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (!_trueTap) return;

                _screenPosition = Input.mousePosition;

                Ray ray = _camera.ScreenPointToRay(_screenPosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100, groundLayer))
                {
                    if (hit.collider.CompareTag("Ground"))
                    {
                        _worldPosition = hit.point;
                    }
                }

                transform.position = _worldPosition;
                
                if (_currentScale <= maxScale)
                {
                    _currentScale += _currentSpeed * Time.deltaTime;
                }

                if (_currentSpeed > minSpeed)
                {
                    _currentSpeed -= acceleration * Time.deltaTime;
                }
                
                transform.localScale = Vector3.one * _currentScale;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (!_trueTap) return;
                
                transform.localScale = Vector3.zero;
                _currentScale = 0;
                _currentSpeed = maxSpeed;
                
                _trueTap = false;
            }
        }
    }
}