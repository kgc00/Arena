using System;
using Abilities;
using Controls;
using Units;
using UnityEngine;

namespace State.PlayerStates
{
    public class ActingUnitState : PlayerState
    {
        public Ability Ability { get; private set; }
        public Vector3 TargetLocation;
        private bool hasActivated;

        public ActingUnitState(Unit owner, Ability ability, Vector3 targetLocation) : base(owner)
        {
            TargetLocation = targetLocation;
            Ability = ability;
        }

        public override void Enter()
        {
                ActivateAbility();
        }

        private void ActivateAbility() {
            Debug.Log("Activate ability = " + hasActivated);
            Owner.AbilityComponent.Activate(Ability, TargetLocation);
            hasActivated = true;
        }

        public override void Exit()
        { }

        public override UnitState HandleUpdate(InputValues input)
        {
            Debug.Log(Owner.AbilityComponent.State);
            Debug.Log(hasActivated);
            if (Owner.AbilityComponent.State == AbilityComponentState.Idle && hasActivated)
            {
                bool playerIsMoving = Math.Abs(input.Forward) > 0 || Math.Abs(input.Horizontal) > 0;

                return playerIsMoving ? (UnitState) new RunUnitState(Owner) : new IdleUnitState(Owner);
            }

            base.HandleUpdate(input);
            
            return null;
        }
    }
}