using UnityEngine;

namespace Units.Modifiers {
    public class ScrObjModifier<T> where T : ScriptableObject {
        protected T Model;
        protected ScrObjModifier<T> Next;

        public ScrObjModifier() { }
        
        public virtual ScrObjModifier<T> InitializeModifier(T data) {
            Next = null;
            Model = null;
            return this;
        }

        public void Add(ScrObjModifier<T> am) {
            if (Next != null) Next.Add(am);
            else Next = am;
        }

        public virtual void Handle() => Next?.Handle();
    }
}