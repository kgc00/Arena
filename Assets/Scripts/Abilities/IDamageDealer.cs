using System.Collections.Generic;
using Data.Types;
using UnityEngine;

namespace Abilities
{
    public interface IDamageDealer
    {
        [SerializeField]  float Damage { get; }
        [SerializeField] List<ControlType> AffectedFactions { get; }
    }
}