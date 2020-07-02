using Units;
using UnityEngine;

namespace State.ChargingAiStates {
    public class ChargeUnitState : UnitState {
        private Transform targetPlayerTransform;
        public ChargeUnitState(Unit owner, Transform targetPlayerTransform) : base(owner) {
            this.targetPlayerTransform = targetPlayerTransform;
        }
    }
}