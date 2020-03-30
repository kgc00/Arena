using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Abilities.AttackAbilities
{
    public interface IDamageDealer
    {
        
        [SerializeField]  float Damage { get; }
        [SerializeField] List<ControlType> AffectedTargets { get; }
    }
}