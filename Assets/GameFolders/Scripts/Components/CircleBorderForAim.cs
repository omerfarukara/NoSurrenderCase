using UnityEngine;

namespace GameFolders.Scripts.Components
{
    public class CircleBorderForAim : MonoBehaviour
    {
        [SerializeField] private Transform handle;
        [SerializeField] float radius; //radius of *black circle*

        private Camera _camera;
        private Vector3 _centerPosition;
        private float _distance;
        private float _zOffset;
    
        void Start()
        {
            _camera = Camera.main;
            _zOffset = _camera!.WorldToScreenPoint(transform.position).z;
            _centerPosition = handle.transform.position;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                TakeAim();
            }
        }

        private void TakeAim()
        {
            Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _zOffset);
            Vector3 newWorldPosition = _camera.ScreenToWorldPoint(screenPosition);
            
            _distance = Vector3.Distance(newWorldPosition, _centerPosition); //distance from ~green object~ to *black circle*
        
            if (_distance > radius) //If the distance is less than the radius, it is already within the circle.
            {
                Vector3 fromOriginToObject = newWorldPosition - _centerPosition; //~GreenPosition~ - *BlackCenter*
                fromOriginToObject *= radius / _distance; //Multiply by radius //Divide by Distance
                newWorldPosition = _centerPosition + fromOriginToObject; //*BlackCenter* + all that Math
            }
       
            handle.transform.position = newWorldPosition;
        }
    }
}
