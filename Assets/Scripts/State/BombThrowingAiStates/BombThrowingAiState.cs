using Controls;
using Units;

namespace State.BombThrowingAiStates {
    public class BombThrowingAiState : UnitState {
        protected BombThrowingAiState(Unit owner) : base(owner) { }
        public override UnitState HandleUpdate(InputValues input) {
            if (Owner.StatusComponent.IsStunned()) {
                return new StunUnitState(Owner);
            }
            return base.HandleUpdate(input);
        }
    }
}