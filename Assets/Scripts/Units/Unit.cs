using System;
using Abilities;
using Controls;
using State;
using Stats;
using Units.Data;
using UnityEngine;
using Utils;

namespace Units
{
    public class Unit : MonoBehaviour, IDamageable, IAbilityUser, IExperienceUser
    {
        public static Action<Unit> OnDeath = delegate {  };
        public Player.Player Owner { get; private set; }
        public Types type;
        [SerializeField] public Rigidbody Rigidbody { get; private set; }
        [SerializeField] public Animator Animator { get; private set; }
        public UnitState state { get; private set; }
        public Controller controller { get; private set; }
        public InputModifierComponent inputModifierComponent { get; private set; }
        public AbilityComponent AbilityComponent { get; private set; }
        public HealthComponent HealthComponent { get; private set; }
        public ExperienceComponent ExperienceComponent { get; private set; }
        public StatusComponent StatusComponent { get; private set; }
        public CoroutineHelper CoroutineHelper { get; private set; }
        public void OnLevelUp() { }
        public bool Initialized { get; private set; } = false;

        public Unit Initialize (Player.Player owner, UnitData data) {
            // Owner
            this.Owner = owner;

            // Controller
            if (controller == null) controller = GetComponentInChildren<Controller>().Initialize(this);
            
            // Input Modifiers
            if (inputModifierComponent == null)
                inputModifierComponent = gameObject.AddComponent<InputModifierComponent>().Initialize(this);
            
            // RigidBody
            if (Rigidbody == null) Rigidbody = GetComponentInChildren<Rigidbody>();
            
            // Animator
            if (Animator == null) Animator = GetComponentInChildren<Animator>();
            
            // Health
            if (HealthComponent == null) HealthComponent = gameObject.AddComponent<HealthComponent>().Initialize(this, data.health);

            // Abilities
            if (AbilityComponent == null) AbilityComponent= gameObject.AddComponent<AbilityComponent>().Initialize(this, data.abilities);
            
            // Experience
            if (ExperienceComponent == null) ExperienceComponent = gameObject.AddComponent<ExperienceComponent>().Initialize(this, data.experience);
            
            // CoroutineHelper
            if (CoroutineHelper == null) CoroutineHelper = gameObject.AddComponent<CoroutineHelper>().Initialize(this);
            
            // Status 
            if (StatusComponent == null) StatusComponent = gameObject.AddComponent<StatusComponent>().Initialize(this);
            
            // State
            state = StateHelper.StateFromEnum(data.state, this);
            state.Enter ();

            Initialized = true;
            return this;
        }
        
        void Update () {
            if (!Initialized) return;
            
            // handle state
            var newState = state?.HandleUpdate (controller.InputValues);
            if (newState == null) return;
            state.Exit();
            state = newState;
            state.Enter();
        }

        private void FixedUpdate()
        {
            if (!Initialized) return;
            state?.HandleFixedUpdate(controller.InputValues);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!Initialized) return;
            state?.HandleCollisionEnter(other);
        }

        public void UnitDeath()
        {
            OnDeath(this);
            Owner.RemoveUnit(this);
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            state?.HandleDrawGizmos();
        }
    }
}