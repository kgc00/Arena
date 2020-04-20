using System;
using System.Linq;
using Abilities;
using Abilities.AttackAbilities;
using Controls;
using Enums;
using Units;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using Utils;

namespace State.PlayerStates
{
    public class IdleUnitState : PlayerState
    {
        public IdleUnitState(Unit owner) : base(owner) { }
        public override UnitState HandleUpdate(InputValues input)
        {
            base.HandleUpdate(input);

            bool playerIsMoving = Math.Abs(input.Forward) > 0 || Math.Abs(input.Horizontal) > 0;
            if (playerIsMoving) return new RunUnitState(Owner);
            
            return null;
        }
    }
}