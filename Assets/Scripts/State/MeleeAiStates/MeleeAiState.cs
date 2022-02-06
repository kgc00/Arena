using Controls;
using Units;

namespace State.MeleeAiStates {
    public class MeleeAiState : UnitState {
        protected MeleeAiState(Unit owner) : base(owner) { }
        public override UnitState HandleUpdate(InputValues input) {
            if (Owner.StatusComponent.IsStunned()) {
                return new StunUnitState(Owner);
            }
            return base.HandleUpdate(input);
        }
    }
}