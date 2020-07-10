using UnityEngine;

namespace Common {
    public class ScriptableObjectModifier<T> : IIconAssetPath where T : ScriptableObject {
        protected T Model;
        protected ScriptableObjectModifier<T> Next;
        public string IconAssetPath => "";

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