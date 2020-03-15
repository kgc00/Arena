using Controls;
using Units;
using UnityEngine;

namespace State
{
    public class UnitState {
        protected readonly Unit Owner;

        public UnitState (Unit owner) {
            this.Owner = owner;
        }
        public virtual void Enter () { }
        public virtual void Exit () { }
        public virtual UnitState HandleUpdate (InputValues input) { return null; }
        public virtual void HandleFixedUpdate (InputValues input) {  }
        public virtual void HandleCollisionEnter(Collision other){ }
    }
}