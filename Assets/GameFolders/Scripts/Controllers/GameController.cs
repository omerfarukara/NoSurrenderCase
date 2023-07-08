using Game.Scripts.General;
using GameFolders.Scripts.General;
using UnityEngine;

namespace GameFolders.Scripts.Controllers
{
    public class GameController : MonoSingleton<GameController>
    {
        private EventData _eventData;
    
        private void Awake()
        {
            _eventData = Resources.Load("EventData") as EventData;
        }
    }
}
