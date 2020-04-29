using System;
using System.Linq;
using Abilities;
using Abilities.AttackAbilities;
using Controls;
using Enums;
using JetBrains.Annotations;
using UI.Targeting;
using Units;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using Utils;

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
            Debug.Log("Entered");
            Shader.SetGlobalFloat("_IndicatorType", Ability.IndicatorType);
        }

        public override void Exit()
        {
            Debug.Log("Exited");
            Shader.SetGlobalFloat("_IndicatorType", 0);
        }

        public override UnitState HandleUpdate(InputValues input)
        {
            base.HandleUpdate(input);

            if (remainingDuration <= 0f)
            {
                bool playerIsMoving = Math.Abs(input.Forward) > 0 || Math.Abs(input.Horizontal) > 0;

                return playerIsMoving ? (UnitState) new RunUnitState(Owner) : new IdleUnitState(Owner);
            }

            remainingDuration -= Time.deltaTime;
            
            return null;
        }
    }
}