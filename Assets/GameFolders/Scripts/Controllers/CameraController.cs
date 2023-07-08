using GameFolders.Scripts.General;
using GameFolders.Scripts.Managers;
using UnityEngine;

namespace GameFolders.Scripts.Controllers
{
    public class CameraController : MonoBehaviour
    {
        private EventData _eventData;

        private void Awake()
        {
            _eventData = Resources.Load("EventData") as EventData;
        }
    }
}
