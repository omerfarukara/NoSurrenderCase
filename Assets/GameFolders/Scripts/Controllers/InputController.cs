using Game.Scripts.General;
using UnityEngine;

namespace GameFolders.Scripts.Controllers
{
    public class InputController : MonoSingleton<InputController>
    {
        [SerializeField] private Joystick _joystick;

        public float JoystickVertical()
        {
            return _joystick.Vertical;
        }
    
        public float JoystickHorizontal()
        {
            return _joystick.Horizontal;
        }
    }
}
