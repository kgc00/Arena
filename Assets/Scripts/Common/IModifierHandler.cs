using System;
using UnityEngine;

namespace UI.Drafting {
    public interface IModifierHandler<in TModel, in TModifier> 
        where TModel : ScriptableObject {
        void AddModifier(TModel model, TModifier modifier, int cost);
        void RemoveModifier(TModel model, TModifier modifier, int cost);
    }
}