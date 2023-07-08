using UnityEngine;

namespace GameFolders.Scripts.General
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static volatile T _instance = null;

        public static T Instance => _instance;

        protected void Singleton(bool dontDestroyOnLoad = false)
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