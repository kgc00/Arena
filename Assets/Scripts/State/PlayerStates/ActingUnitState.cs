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

        public ActingUnitState(Unit owner, Ability ability, Vector3 targetLocation) : base(owner)
        {
            TargetLocation = targetLocation;
            Ability = ability;
        }

        public override void Enter()
        {
            Owner.AbilityComponent.Activate(Ability, TargetLocation);
            
            Shader.SetGlobalFloat("_IndicatorType", Ability.IndicatorType);
            Debug.Log("Entering acting state");
        }

        public override void Exit()
        {
            Shader.SetGlobalFloat("_IndicatorType", 0);
            Debug.Log("Exiting acting state");
        }

        public override UnitState HandleUpdate(InputValues input)
        {
            if (Owner.AbilityComponent.State == AbilityComponentState.Idle)
            {
                bool playerIsMoving = Math.Abs(input.Forward) > 0 || Math.Abs(input.Horizontal) > 0;

                return playerIsMoving ? (UnitState) new RunUnitState(Owner) : new IdleUnitState(Owner);
            }

            base.HandleUpdate(input);
            
            return null;
        }
    }
}