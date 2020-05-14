using Units;
using UnityEngine;

namespace Controls
{
    public class InputModifierComponent : MonoBehaviour
    { 
        public Unit Owner { get; private set; }
        public InputModifiers InputModifiers { get; private set; } = (InputModifiers) 0;
        public InputModifierComponent Initialize (Unit owner) {
            Owner = owner;
            return this;
        }
        
        public void AddInputModifier(InputModifiers inputModifiers) => InputModifiers |= inputModifiers;
        public void RemoveInputModifier(InputModifiers inputModifiers) => InputModifiers &= ~inputModifiers;
    }
}

