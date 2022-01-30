using System;
using Abilities;
using Data;
using Data.Types;
using Units;
using UnityEngine;

namespace State {
    [Serializable]
    public class UnitIntent {
        [SerializeField] public Ability ability;
        [SerializeField] public TargetingData targetingData;
        [SerializeField] public Unit unit;

        public UnitIntent(Ability ability, TargetingData targetingData, Unit unit) {
            this.ability = ability;
            this.targetingData = targetingData;
            this.unit = unit;
        }
    }
}