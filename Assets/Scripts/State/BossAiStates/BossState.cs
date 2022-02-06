using Controls;
using State.MeleeAiStates;
using Units;
using UnityEngine;

namespace State.BossAiStates {
    public class BossState : UnitState {
        protected BossState(Unit owner) : base(owner) { }
        public override UnitState HandleUpdate(InputValues input) {
            if (Owner.StatusComponent.IsStunned()) {
                return new StunUnitState(Owner);
            }
            return base.HandleUpdate(input);
        }
    }
}