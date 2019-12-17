using Enums;
using UnityEngine.InputSystem;

namespace Controls
{
    public class InputValues
    {
        public float Forward;
        public float Horizontal;
        public float Look;
        public float Turn;
        public ControllerType ActiveControl { get; private set; }

        public float Fire;
        public InputActionPhase FirePhase;

        public InputValues()
        {
            Forward = 0;
            Horizontal = 0;
            Look = 0;
            Turn = 0;
            Fire = 0;
            ActiveControl = ControllerType.None;
        }
        
        public InputValues(float forward,
            float horizontal,
            float look,
            float turn,
            float fire,
            InputActionPhase firePhase,
            ControllerType activeControl)
        {
            Forward = forward;
            Horizontal = horizontal;
            Look = look;
            Turn = turn;
            Fire = fire;
            FirePhase = firePhase;
            ActiveControl = activeControl;
        }

        public void SetControlType(string displayName)
        {
            if (displayName == "Delta")
            {
                ActiveControl = ControllerType.Delta;
            }else if (displayName == "Right Stick")
            {
                ActiveControl = ControllerType.GamePad;
            }
        }
    }
}