using System.Collections;
using UnityEngine;

namespace GameFolders.Scripts.SpawnSystem
{
    public class SampleObject : SpawnObject
    {
        [SerializeField] private float lifeTime;
    
        protected override void OnStartTask()
        {
            StartCoroutine(CloseObject());
        }

        protected override void CompleteTask()// Call this method when the object task is done
        {
            base.CompleteTask();
        }
        
        private IEnumerator CloseObject()
        {
            yield return new WaitForSeconds(lifeTime);
            CompleteTask(); 
        }
    }
}
