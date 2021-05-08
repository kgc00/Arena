using System;
using Units;
using UnityEngine;

namespace Controls
{
    public abstract class Controller : MonoBehaviour {
        [field: SerializeField] public bool QuickCast { get; protected set; } 
        public Unit Owner { get; protected set; }
        public virtual InputValues InputValues { get; protected set; } = new InputValues();
        
        public virtual void HandleUpdate(){}

        // Reset values must be called in late update (after subscribers of the input have received the values for the frame)
        // This is how we accurately determine which frame a key was pressed down.
        private void LateUpdate()
        {
            InputValues.ResetValues();
        }

        public Controller Initialize(Unit unit) {
            Owner = unit;
            return this;
        }
    }
}