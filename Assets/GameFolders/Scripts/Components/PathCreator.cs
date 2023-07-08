using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameFolders.Scripts.Components
{
    public class PathCreator : MonoBehaviour
    {
        [SerializeField] private List<Transform> wayPoints;
        [SerializeField] private Vector3 newLocalPosition;
        [SerializeField] private Color color = Color.green;
    
        [Button("Add Point")]
        private void AddPoint()
        {
            GameObject newObject = new GameObject
            {
                transform =
                {
                    parent = transform,
                    localPosition = newLocalPosition
                },
                name = $"Point {wayPoints.Count}"
            };
            wayPoints.Add(newObject.transform);
        }

        [Button("Change Position Last Point")]
        private void ChangePositionLastPoint()
        {
            wayPoints[^1].localPosition = newLocalPosition;
        }    

        [Button("Remove Last Point")]
        private void DeleteLast()
        {
            Transform current = wayPoints[^1];
            wayPoints.Remove(current);
            DestroyImmediate(current.gameObject);
        }

        [Button("Clear Points")]
        private void ClearPoints()
        {
            int count = wayPoints.Count;

            for (int i = 0; i < count; i++)
            {
                Transform current = wayPoints[^1];
                wayPoints.Remove(current);
                DestroyImmediate(current.gameObject);
            }
        }

        public List<Transform> WayPoints
        {
            get => wayPoints;
            set => wayPoints = value;
        }

        private void OnDrawGizmos()
        {
            if (wayPoints.Count == 0) return;

            Gizmos.color = color;
        
            foreach (Transform wayPoint in wayPoints)
            {
                Gizmos.DrawSphere(wayPoint.position, 0.1f);
            }
        }
    }
}
