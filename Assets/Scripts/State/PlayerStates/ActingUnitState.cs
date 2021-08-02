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
            Debug.Log("Activate ability = " + _hasActivated);
            Owner.AbilityComponent.Activate(ref _ability, _targetLocation);
            _hasActivated = true;
        }

        public override void Exit()
        { }

        public override UnitState HandleUpdate(InputValues input)
        {
            Debug.Log(Owner.AbilityComponent.State);
            Debug.Log(_hasActivated);
            if (Owner.AbilityComponent.State == AbilityComponentState.Idle && _hasActivated)
            {
                bool playerIsMoving = Math.Abs(input.Forward) > 0 || Math.Abs(input.Horizontal) > 0;

                return playerIsMoving ? (UnitState) new RunUnitState(Owner) : new IdleUnitState(Owner);
            }

            base.HandleUpdate(input);
            
            return null;
        }
    }
}