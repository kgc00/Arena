using System.Collections;
using Abilities;
using Controls;
using Units;

namespace State.BossAiStates {
    public abstract class AbilityUnitState<T> : UnitState where T : Ability {
        protected bool abilityFinished;
        protected T ability;

        protected AbilityUnitState(Unit owner) : base(owner) {
            ability = Owner.AbilityComponent.GetEquippedAbility<T>();
            Ability.OnAbilityFinished += HandleAbilityFinished;
        }
        
        ~AbilityUnitState() => Ability.OnAbilityFinished -= HandleAbilityFinished;

        public override void Enter() => Owner.CoroutineHelper.SpawnCoroutine(HandleAbility());

        protected virtual void HandleAbilityFinished(Unit u, Ability a) {
            if (u != Owner || a != ability) return;

            abilityFinished = true;
        }

        protected abstract IEnumerator HandleAbility();
    }
}