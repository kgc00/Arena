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
        public Player Owner { get; private set; }
        public Units.Types type;
        [SerializeField] public Rigidbody Rigidbody { get; private set; }
        [SerializeField] public Animator Animator { get; private set; }
        UnitState state;
        Controller controller;
        public AbilityComponent AbilityComponent { get; private set; }
        public HealthComponent HealthComponent { get; private set; }
        public ExperienceComponent ExperienceComponent { get; private set; }
        public CoroutineHelper CoroutineHelper { get; private set; }
        public void OnLevelUp() { }
        public bool Initialized { get; private set; } = false;

        public Unit Initialize (Player owner, UnitData data) {
            //Owner
            this.Owner = owner;

            //Controller
            if (controller == null) controller = GetComponentInChildren<Controller>();
            
            //RigidBody
            if (Rigidbody == null) Rigidbody = GetComponentInChildren<Rigidbody>();
            
            // Animator
            if (Animator == null) Animator = GetComponentInChildren<Animator>();
            
            // Health
            if (HealthComponent == null) HealthComponent = gameObject.AddComponent<HealthComponent>().Initialize(this, data.health);

            // Abilities
            if (AbilityComponent == null)AbilityComponent= gameObject.AddComponent<AbilityComponent>().Initialize(this, data.abilities);
            
            //Experience
            if (ExperienceComponent == null) ExperienceComponent = gameObject.AddComponent<ExperienceComponent>().Initialize(this, data.experience);
            
            // CoroutineHelper
            if (CoroutineHelper == null) CoroutineHelper = gameObject.AddComponent<CoroutineHelper>().Initialize(this);
            
            //State
            state = StateHelper.StateFromEnum(data.state, this);
            state.Enter ();

            Initialized = true;
            return this;
        }

        void Test() => Debug.Log("Called");

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
            Destroy(gameObject, 0.1f);
        }
    }
}