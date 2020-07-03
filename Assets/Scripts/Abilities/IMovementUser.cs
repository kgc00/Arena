using System;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Abilities {
    public interface IMovementUser {
        [SerializeField]  float MovementSpeedModifier { get; }
        [SerializeField] Action<Unit, Ability> DestinationReached { get; }
    }
}