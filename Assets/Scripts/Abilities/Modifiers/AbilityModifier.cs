using System;
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
    public class AbilityModifier
    {
        protected Ability Ability;
        protected AbilityModifier Next;
        public virtual bool ShouldConsume() => true;

        public AbilityModifier(Ability ability)
        {
            this.Ability = ability;
        }

        /// <summary>
        /// When building the link list to traverse this method must be called.  This will:
        /// 1:)  Clear any previously stored linked list data
        /// 2.) Assign the ability being modified 
        /// </summary>
        /// <param name="ability"></param>
        /// <returns></returns>
        public virtual AbilityModifier InitializeModifier(Ability ability)
        {
            Next = null;
            Ability = null;
            return this;
        }

        public void Add(AbilityModifier am)
        {
            if (Next != null) Next.Add(am);
            else Next = am;
        }

        public virtual void Handle() => Next?.Handle();
    }
}