using Data.Types;
using UnityEngine.InputSystem;

namespace Controls
{
    public class ButtonValues
    {
        public ButtonType ButtonType;
        public float PressValue;
        public InputActionPhase PressPhase;
        public bool HasPerformedPress { get; set; }
        public bool HasReleasedPress { get; set; }
        public IInputInteraction PressInteraction { get; set; }

        public ButtonValues(ButtonType type)
        {
            PressValue = 0;
            PressPhase = InputActionPhase.Waiting;
            HasPerformedPress = false;
            HasReleasedPress = false;
            PressInteraction = null;
            ButtonType = type;
        }
    }
}