using Controls;
using Units;

namespace State.ChargingAiStates {
    public class ChargeAiState : UnitState {
        protected ChargeAiState(Unit owner) : base(owner) { }
        public override UnitState HandleUpdate(InputValues input) {
            if (Owner.StatusComponent.IsStunned()) {
                return new StunUnitState(Owner);
            }
            return base.HandleUpdate(input);
        }
    }
}