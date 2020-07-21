using UnityEngine;

namespace UI.Drafting {
    public interface IModifierHandler<TModel, TModifier> where TModel : ScriptableObject where TModifier : new () {
        void AddModifier(TModel model, TModifier modifier);
        void RemoveModifier(TModel model, TModifier modifier);
    }
}