using System.Collections.Generic;
using Units;

namespace Spawner {
    public interface IUnitInterval : IInterval
    {
        float CurrentDelay { get; }
        List<Unit> CurrentWave { get; }
    }
}