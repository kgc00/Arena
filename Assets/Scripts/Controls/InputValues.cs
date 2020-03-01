﻿using Enums;
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
        public bool HasStartedFire { get; set; }
        public IInputInteraction FireInteraction { get; set; }

        public InputValues()
        {
            Forward = 0;
            Horizontal = 0;
            Look = 0;
            Turn = 0;
            Fire = 0;
            ActiveControl = ControllerType.None;
            HasStartedFire = false;
            FireInteraction = null;
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
            Fire = fire;
            FirePhase = firePhase;
            ActiveControl = activeControl;
            HasStartedFire = hasStartedFire;
            FireInteraction = fireInteraction;
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
            this.HasStartedFire = false;
        }
    }
}