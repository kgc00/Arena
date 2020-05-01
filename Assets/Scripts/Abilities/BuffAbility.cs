using System.Collections.Generic;
using Abilities.Data;
using Enums;
using Units;
using UnityEngine;

namespace Abilities
{
    public abstract class BuffAbility : Ability, IBuffUser
    {
        public float Duration { get; protected set; }
        public List<ControlType> AffectedFactions { get; protected set; }
        public virtual Ability Initialize(BuffAbilityData data, Unit owner)
        {
            Owner = owner;
            Range = data.range;
            AreaOfEffectRadius = data.areaOfEffectRadius;
            AffectedFactions = data.AffectedFactions;
            Cooldown = new Cooldown(data.cooldown);
            StartupTime = data.startupTime;
            IndicatorType = data.indicatorType;
            Duration = data.Duration;
            return this;
        }

    }
}