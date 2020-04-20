using System.Collections.Generic;
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

        public Dictionary<ButtonType, ButtonValues> ButtonValues;
            

        public InputValues()
        {
            Forward = 0;
            Horizontal = 0;
            Look = 0;
            Turn = 0;
            ActiveControl = ControllerType.None;
            ButtonValues = new Dictionary<ButtonType, ButtonValues>()
            {
                [ButtonType.Skill1] = new ButtonValues(ButtonType.Skill1),
                [ButtonType.Skill2] = new ButtonValues(ButtonType.Skill2),
                [ButtonType.Skill3] = new ButtonValues(ButtonType.Skill3),
                [ButtonType.Skill4] = new ButtonValues(ButtonType.Skill4)
            };
        }
        
        public InputValues(float forward,
            float horizontal,
            float look,
            float turn,
            float fire,
            InputActionPhase firePhase,
            ControllerType activeControl,
            bool hasStartedFire, 
            IInputInteraction fireInteraction)
        {
            Forward = forward;
            Horizontal = horizontal;
            Look = look;
            Turn = turn;
            ActiveControl = activeControl;
            ButtonValues = new Dictionary<ButtonType, ButtonValues>()
            {
                [ButtonType.Skill1] = new ButtonValues(ButtonType.Skill1),
                [ButtonType.Skill2] = new ButtonValues(ButtonType.Skill2),
                [ButtonType.Skill3] = new ButtonValues(ButtonType.Skill3),
                [ButtonType.Skill4] = new ButtonValues(ButtonType.Skill4)
            };
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

        public virtual void ResetValues()
        {
            foreach (var keyValuePair in ButtonValues)
            {
                keyValuePair.Value.HasStartedPress = false;
            }
        }
    }
}