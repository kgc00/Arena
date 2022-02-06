using Controls;
using State.MeleeAiStates;
using Units;

namespace State.RangedAiStates {
    public class RangedAiState : UnitState {
        protected RangedAiState(Unit owner) : base(owner) { }
        public override UnitState HandleUpdate(InputValues input) {
            if (Owner.StatusComponent.IsStunned()) {
                return new StunUnitState(Owner);
            }
            return base.HandleUpdate(input);
        }
    }
}