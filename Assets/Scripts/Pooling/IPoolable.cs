using UnityEngine;

namespace Pooling {
    public interface IPoolable {
        void HandleExitFromPool();
        void HandleReturnToPool();
        bool inUse { get; set; }
        GameObject Owner { get; }
        string poolKey { get; set; }
    }
}