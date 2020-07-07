using UnityEngine;

namespace Common {
    public interface IInitializable<TModel, out TObj> where TModel : ScriptableObject {
        TModel Model { get; }
        TObj Initialize(TModel model);
    }
}