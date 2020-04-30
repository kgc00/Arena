using System;
using Abilities;
using Controls;
using Units;
using UnityEngine;

namespace State.PlayerStates
{
    public class ActingUnitState : PlayerState
    {
        public float remainingDuration { get; private set; }
        public Ability Ability { get; private set; }

        public ActingUnitState(Unit owner, Ability ability) : base(owner)
        {
            Ability = ability;
            remainingDuration = ability.StartupTime;
        }

        public override void Enter()
        {
            Shader.SetGlobalFloat("_IndicatorType", Ability.IndicatorType);
        }

        public override void Exit()
        {
            Shader.SetGlobalFloat("_IndicatorType", 0);
        }

        public override UnitState HandleUpdate(InputValues input)
        {
            if (remainingDuration <= 0f)
            {
                bool playerIsMoving = Math.Abs(input.Forward) > 0 || Math.Abs(input.Horizontal) > 0;

                return playerIsMoving ? (UnitState) new RunUnitState(Owner) : new IdleUnitState(Owner);
            }

            base.HandleUpdate(input);
            
            remainingDuration -= Time.deltaTime;
            
            return null;
        }
    }
}