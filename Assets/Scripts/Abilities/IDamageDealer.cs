using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Abilities
{
    public interface IDamageDealer
    {
        
        [SerializeField]  float Damage { get; }
        [SerializeField] List<ControlType> AffectedFactions { get; }
    }
}