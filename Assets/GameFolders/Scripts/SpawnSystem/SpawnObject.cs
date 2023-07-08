using UnityEngine;

namespace GameFolders.Scripts.SpawnSystem
{
    public abstract class SpawnObject : MonoBehaviour
    {
        protected Spawner Spawner { get; private set; }

        public void Initialize(Spawner spawner)
        {
            Spawner = spawner;
        }

        public void WakeUp(Vector3 spawnPosition)
        {
            transform.position = spawnPosition;
            OnStartTask();
        }

        protected abstract void OnStartTask();

        protected virtual void CompleteTask()
        {
            Spawner.AddQueue(this);
            gameObject.SetActive(false);
        }
    }
}