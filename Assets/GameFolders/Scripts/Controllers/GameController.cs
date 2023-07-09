using System.Collections.Generic;
using Game.Scripts.General;
using GameFolders.Scripts.Controllers.AI;
using GameFolders.Scripts.General;
using UnityEngine;

namespace GameFolders.Scripts.Controllers
{
    public class GameController : MonoSingleton<GameController>
    {
        [SerializeField] internal List<AIController> _ais;
    }
}
