using GameFolders.Scripts.General;
using UnityEngine;

namespace GameFolders.Scripts.Controllers
{
    public class GameController : MonoSingleton<GameController>
    {
        private EventData _eventData;
    
        private void Awake()
        {
            Singleton();
            _eventData = Resources.Load("EventData") as EventData;
        }
    }
}
