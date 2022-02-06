using System;
using System.Collections;
using Abilities;
using Units;

namespace State {
    public abstract class AbilityUnitState<T> : UnitState where T : Ability {
        protected bool AbilityFinished;
        protected Ability Ability;

        protected AbilityUnitState(Unit owner) : base(owner) {
            Ability = Owner.AbilityComponent.GetEquippedAbility<T>();
            if (Ability == null) {
                throw new Exception("Ability must be assigned in AbilityUnitState constructor");
            }
            AbilityFinished = false;
            if (!Ability.OnAbilityFinished.Contains(HandleAbilityFinished)) {
                Ability.OnAbilityFinished.Insert(0, HandleAbilityFinished);
            }
        }

        public override void Exit() {
            if (Ability.OnAbilityFinished.Contains(HandleAbilityFinished)) {
                Ability.OnAbilityFinished.Remove(HandleAbilityFinished);
            }
        }
        
        public override void Enter() {
            Owner.CoroutineHelper.SpawnCoroutine(HandleAbility());
        }

        protected virtual void HandleAbilityFinished(Unit u, Ability a) {
            AbilityFinished = true;
        }

        protected abstract IEnumerator HandleAbility();
    }
}