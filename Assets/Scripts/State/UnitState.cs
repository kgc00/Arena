using Controls;
using Units;
using UnityEngine;
using UnityEngine.InputSystem;

namespace State
{
    public class UnitState {
        protected readonly Unit Owner;

        public UnitState (Unit owner) {
            this.Owner = owner;
        }
        public virtual void Enter () { }
        public virtual UnitState HandleUpdate () { return null; }
        public virtual void HandleFixedUpdate () {  }
        public virtual void HandleCollisionEnter(Collision other){ }
        public virtual void HandleOnFire(InputAction.CallbackContext context) { }
        public virtual void HandleOnLook(InputAction.CallbackContext context) { }
        public virtual void HandleOnMove(InputAction.CallbackContext context) { }
    }
}