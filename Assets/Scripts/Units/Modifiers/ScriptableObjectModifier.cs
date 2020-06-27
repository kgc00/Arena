using UnityEngine;

namespace Units.Modifiers {
    public class ScriptableObjectModifier<T> where T : ScriptableObject {
        protected T Model;
        protected ScriptableObjectModifier<T> Next;

        public ScriptableObjectModifier() { }
        
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