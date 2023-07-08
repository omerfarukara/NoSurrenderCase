using UnityEngine;

namespace GameFolders.Scripts.Components
{
    public class CloseAfterLifeTime : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 1f;
        
        private float _currentTime;

        private void Update()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= lifeTime)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
