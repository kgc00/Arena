using System.Collections.Generic;
using Data.Types;
using UnityEngine;

namespace Abilities
{
    public interface IBuffUser
    {
        [SerializeField] float Duration { get; }
        [SerializeField] List<ControlType> AffectedFactions { get; }
    }
}