using Units;
using UnityEngine;

namespace Controls
{
    public class InputModifierComponent : MonoBehaviour
    { 
        public Unit Owner { get; private set; }
        public InputModifier InputModifier { get; private set; } = (InputModifier) 0;
        public InputModifierComponent Initialize (Unit owner) {
            Owner = owner;
            return this;
        }
        
        public InputModifierComponent AddModifier(InputModifier inputModifier) {
            InputModifier |= inputModifier;
            return this;
        }

        public InputModifierComponent RemoveModifier(InputModifier inputModifier) {
             InputModifier &= ~inputModifier;
             return this;
        }
    }
}

