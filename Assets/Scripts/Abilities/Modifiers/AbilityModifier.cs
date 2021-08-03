using System;
using Data.Modifiers;
using UnityEngine;

namespace Abilities.Modifiers
{
    /// <summary>
    /// A modified Chain of Responsibility pattern.  Because we do not know which ability
    /// we are modifying when this class is created, we have an InitializeModifier method
    /// which clears any previous data (useful for persistent / multi-use modifiers),
    /// and assigns the values required for the modifiers to function.
    ///
    /// Chain of Responsibility: a class with a linked list and a reference to another class.
    /// Each node on the linked list is called sequentially, modifying the referenced class.
    /// </summary>
    public class AbilityModifier: IEquatable<AbilityModifier>  {
        protected AbilityModifier Next;
        public AbilityModifierType Type;
        
        public virtual bool ShouldConsume()
        {
            Debug.Log($"Consuming: Some Modifier");
            return true;
        }

        public AbilityModifier(Ability ability) {
            Type = AbilityModifierType.BaseAbilityModifier;
        }

        /// <summary>
        /// When building the link list to traverse this method must be called.  This will:
        /// 1:)  Clear any previously stored linked list data
        /// 2.) Assign the ability being modified 
        /// </summary>
        /// <param name="ability"></param>
        /// <returns name="AbilityModifier"></returns>
        public virtual AbilityModifier InitializeModifier(Ability ability)
        {
            Next = null;
            return this;
        }

        public void Add(AbilityModifier am)
        {
            if (Next != null) Next.Add(am);
            else Next = am;
        }

        public virtual void Handle() => Next?.Handle();

        public bool Equals(AbilityModifier other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Type == other.Type;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AbilityModifier) obj);
        }

        public override int GetHashCode() {
            return (int) Type;
        }
    }
}