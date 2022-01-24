using Controls;
using Units;
using UnityEngine;
using static Utils.MathHelpers;

namespace State.ChargingAiStates {
    public class StunUnitState : UnitState {
        private float timeLeft;
        private readonly float stunDuration;
        private static readonly int Idle = Animator.StringToHash("Idle");

        public StunUnitState(Unit owner, float stunDuration) : base(owner) {
            timeLeft = stunDuration;
            this.stunDuration = stunDuration;
        }

        public override void Enter()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.SetTrigger(Idle);
        }

        public override void Exit()
        {
            if (Owner.Animator == null || !Owner.Animator) return;
            Owner.Animator.ResetTrigger(Idle);
        }

        public override UnitState HandleUpdate(InputValues input) {
            timeLeft = Clamp(timeLeft - Time.deltaTime, 0, stunDuration);
            
            if (timeLeft > 0) return null;
            return new IdleUnitState(Owner); 
        }
    }
}