using System;
using UnityEngine;

namespace Common {
    [Serializable]
    public class ScriptableObjectModifier<T> : IIconAssetPath where T : ScriptableObject {
        protected T Model;
        protected ScriptableObjectModifier<T> Next;
        public virtual string IconAssetPath() => "base";
        public virtual string DisplayText() => "null";

        public virtual ScriptableObjectModifier<T> InitializeModifier(T data) {
            Next = null;
            Model = null;
            return this;
        }

        public void Add(ScriptableObjectModifier<T> am) {
            if (Next != null) Next.Add(am);
            else Next = am;
        }

        public virtual void Handle() => Next?.Handle();
    }
}