using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;
using GameFolders.Scripts.General.FGEnum;
using ValueType = GameFolders.Scripts.General.FGEnum.ValueType;

namespace GameFolders.Scripts.SpawnSystem
{
    public class Spawner : MonoBehaviour
    {
        [Header("General Settings")] [SerializeField]
        private SpawnObject[] objects;

        [SerializeField] private float yOffset = 0f;

        [Header("Time Settings")] [SerializeField]
        private ValueType spawnTimeType;

        [ShowIf("spawnTimeType", ValueType.Range)] [SerializeField]
        private Vector2 spawnTimeRange;

        [ShowIf("spawnTimeType", ValueType.Constant)] [SerializeField]
        private float spawnTimeConstant;

        [Header("Area Border Settings")] [SerializeField]
        private BorderType areaBorderType;

        [ShowIf("areaBorderType", BorderType.Rectangle)] [SerializeField]
        private Vector2 xBorders;

        [ShowIf("areaBorderType", BorderType.Rectangle)] [SerializeField]
        private Vector2 zBorders;

        [ShowIf("areaBorderType", BorderType.Circle)] [SerializeField]
        private float radius;

        [ShowIf("areaBorderType", BorderType.HallowCircle)] [SerializeField]
        private float innerRadius;

        [ShowIf("areaBorderType", BorderType.HallowCircle)] [SerializeField]
        private float outerRadius;

        [SerializeField] private Color borderGizmosColor = Color.black;
        [SerializeField] private float tolerance;

        private readonly Queue<SpawnObject> _pool = new Queue<SpawnObject>();

        private float _spawnTime;

        private void Start()
        {
            SetNewSpawnTime();
        }

        private void Update()
        {
            SpawnTimeControl();
        }

        public void AddQueue(SpawnObject spawnObject)
        {
            _pool.Enqueue(spawnObject);
        }

        private void SetNewSpawnTime()
        {
            _spawnTime = spawnTimeType switch
            {
                ValueType.Constant => spawnTimeConstant,
                ValueType.Range => Random.Range(spawnTimeRange.x, spawnTimeRange.y),
                _ => _spawnTime
            };
        }

        private void SpawnTimeControl()
        {
            _spawnTime -= Time.deltaTime;

            if (_spawnTime <= 0)
            {
                SpawnNewObject();
                SetNewSpawnTime();
            }
        }

        private void SpawnNewObject()
        {
            switch (_pool.Count)
            {
                case >= 1000:
                    throw new Exception("System is overloaded");
                case 0:
                {
                    int randomIndex = Random.Range(0, objects.Length);

                    SpawnObject newSpawnObject = Instantiate(objects[randomIndex]);

                    newSpawnObject.Initialize(this);
                    _pool.Enqueue(newSpawnObject);
                    break;
                }
            }

            SpawnObject spawnObject = _pool.Dequeue();
            spawnObject.gameObject.SetActive(true);
            spawnObject.WakeUp(GetNewPosition());
        }

        private Vector3 GetNewPosition()
        {
            float x, z;
            Vector3 newPosition;
            
            switch (areaBorderType)
            {
                case BorderType.Rectangle:

                    x = Random.Range(xBorders.x, xBorders.y);
                    z = Random.Range(zBorders.x, zBorders.y);

                    return new Vector3(x, yOffset, z);

                case BorderType.Circle:

                    x = Random.Range(-radius, radius);
                    z = Random.Range(-radius, radius);

                    newPosition = new Vector3(x, 0, z);

                    if (Vector3.Distance(newPosition, Vector3.zero) > radius)
                    {
                        float differance = Vector3.Distance(newPosition, Vector3.zero) - radius;
                        
                        differance = Random.Range(differance, radius + differance);
                        newPosition.x = newPosition.x > 0 ? newPosition.x - differance : newPosition.x + differance;
                        newPosition.z = newPosition.z > 0 ? newPosition.z - differance : newPosition.z + differance;
                    }

                    newPosition.y = yOffset;

                    return newPosition;

                case BorderType.HallowCircle:
                    
                    x = Random.Range(-outerRadius, outerRadius);
                    z = Random.Range(-outerRadius, outerRadius);

                    newPosition = new Vector3(x, 0, z);
                    
                    float distance = Vector3.Distance(newPosition, Vector3.zero);
                    float deltaDistance, fixedDistance, moveDistance;
                    
                    if (distance > outerRadius)
                    {
                        deltaDistance = distance - outerRadius;
                        fixedDistance = (outerRadius - innerRadius) / Mathf.Sqrt((outerRadius - innerRadius));
                        moveDistance = Random.Range(deltaDistance, fixedDistance);
                        newPosition.x = newPosition.x > 0 ? newPosition.x - moveDistance : newPosition.x + moveDistance;
                        newPosition.z = newPosition.z > 0 ? newPosition.z - moveDistance : newPosition.z + moveDistance;
                    }
                    else if (distance < innerRadius)
                    {
                        deltaDistance = innerRadius - distance;
                        fixedDistance = (outerRadius - innerRadius) / Mathf.Sqrt((outerRadius - innerRadius));
                        moveDistance = Random.Range(deltaDistance, fixedDistance);
                        newPosition.x = newPosition.x > 0 ? newPosition.x + moveDistance : newPosition.x - moveDistance;
                        newPosition.z = newPosition.z > 0 ? newPosition.z + moveDistance : newPosition.z - moveDistance;
                    }

                    newPosition.y = yOffset;

                    return newPosition;

                default:
                    return transform.position;
            }
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            switch (areaBorderType)
            {
                case BorderType.Rectangle:
                    float centerX = transform.position.x + (xBorders.x + xBorders.y) / 2;
                    float centerZ = transform.position.z + (zBorders.x + zBorders.y) / 2;
                    Vector3 center = new Vector3(centerX, transform.position.y, centerZ);
                    float sizeX = Mathf.Abs(xBorders.x) + Mathf.Abs(xBorders.y);
                    float sizeZ = Mathf.Abs(zBorders.x) + Mathf.Abs(zBorders.y);
                    Vector3 size = new Vector3(sizeX, 0.01f, sizeZ);
                    Gizmos.color = borderGizmosColor;
                    Gizmos.DrawCube(center, size);
                    break;
                case BorderType.Circle:
                    UnityEditor.Handles.color = borderGizmosColor;
                    UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.up, radius);
                    break;
                case BorderType.HallowCircle:
                    UnityEditor.Handles.color = borderGizmosColor;
                    UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.up, outerRadius);
                    UnityEditor.Handles.color = Color.white;
                    UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.up, innerRadius);
                    break;
            }
#endif
        }
    }
}