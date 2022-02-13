using System;
using Abilities;
using Controls;
using Units;
using UnityEngine;

namespace State.PlayerStates
{
    public class ActingUnitState : PlayerState {
        private Ability _ability;
        private Vector3 _targetLocation;
        private bool _hasActivated;

        public ActingUnitState(Unit owner, Ability ability, Vector3 targetLocation) : base(owner)
        {
            _targetLocation = targetLocation;
            _ability = ability;
        }

        public override void Enter()
        {
                ActivateAbility();
        }

        private void ActivateAbility() {
            Owner.AbilityComponent.Activate(ref _ability, _targetLocation);
            _hasActivated = true;
        }

        public override void Exit()
        { }

        public override UnitState HandleUpdate(InputValues input)
        {
            if (Owner.AbilityComponent.State == AbilityComponentState.Idle && _hasActivated)
            {
                bool playerIsMoving = Math.Abs(input.Forward) > 0 || Math.Abs(input.Horizontal) > 0;

                return playerIsMoving ? (UnitState) new RunUnitState(Owner) : new IdleUnitState(Owner);
            }
            
            return base.HandleUpdate(input);
        }
    }
}