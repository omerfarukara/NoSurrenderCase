using UnityEngine;

namespace Game.Scripts.General
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static volatile T _instance = null;

        public static T Instance => _instance;

        [SerializeField] private bool dontDestroyOnLoad = false;

        private void Awake()
        {
            Singleton();
        }

        private void Singleton()
        {
            if (dontDestroyOnLoad)
            {
                if (_instance == null)
                {
                    _instance = this as T;
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (_instance == null)
                {
                    _instance = this as T;
                }
            }
        }
    }
}