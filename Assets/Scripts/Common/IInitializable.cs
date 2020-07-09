using UnityEngine;

namespace Common {
    public interface IInitializable<TModel, TOwner, out TSelf> where TModel : ScriptableObject {
        TOwner Owner { get; }
        TModel Model { get; }
        TSelf Initialize(TModel m, TOwner o);
        bool Initialized { get; }
    }
}