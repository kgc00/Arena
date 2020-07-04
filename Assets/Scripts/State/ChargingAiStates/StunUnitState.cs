using Controls;
using Units;
using UnityEngine;
using static Utils.MathHelpers;

namespace State.ChargingAiStates {
    public class StunUnitState : UnitState {
        private float timeLeft;
        private readonly float stunDuration;

        public StunUnitState(Unit owner, float stunDuration) : base(owner) {
            timeLeft = stunDuration;
            this.stunDuration = stunDuration;
        }

        public override UnitState HandleUpdate(InputValues input) {
            timeLeft = Clamp(timeLeft - Time.deltaTime, 0, stunDuration);
            
            if(timeLeft > 0) return null;
            return new IdleUnitState(Owner); 
        }
    }
}