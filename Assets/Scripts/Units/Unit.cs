using System;
using Abilities;
using Controls;
using State;
using State.PlayerStates;
using Stats;
using UnityEngine;
using Utils;

namespace Units
{
    public class Unit : MonoBehaviour, IDamageable, IAbilityUser, IExperienceUser
    {
        public static Action<Unit> OnDeath = delegate {  };
        public Player Owner { get; private set; }
        public BaseStats BaseStats { get; private set; }
        public Stats.Stats initStatValues;
        [SerializeField] public Rigidbody Rigidbody { get; private set; }
        [SerializeField] public Animator Animator { get; private set; }
        UnitState state;
        [SerializeField] private UnitStateEnum stateBehaviour;
        Controller controller;
        public AbilityComponent AbilityComponent { get; private set; }
        public HealthComponent HealthComponent { get; private set; }
        public ExperienceComponent ExperienceComponent { get; private set;  }
        public bool Initialized { get; private set; } = false;

        public Unit Initialize (Player owner) {
            //Owner
            this.Owner = owner;

            //Controller
            if (controller == null) controller = GetComponentInChildren<Controller>();
            
            //RigidBody
            if (Rigidbody == null) Rigidbody = GetComponentInChildren<Rigidbody>();
            
            // Animator
            if (Animator == null) Animator = GetComponentInChildren<Animator>();
            
            //Stats
            if(initStatValues == null) throw new Exception("Initial Stat Values must be set.");
            if (BaseStats == null)BaseStats = new BaseStats(initStatValues);
            
            // Health
            if (HealthComponent == null) HealthComponent = gameObject.AddComponent<HealthComponent>().Initialize(this);

            // Abilities
            if (AbilityComponent == null)AbilityComponent= gameObject.AddComponent<AbilityComponent>().Initialize(this);
            
            //Experience
            if (ExperienceComponent == null) ExperienceComponent = gameObject.AddComponent<ExperienceComponent>().Initialize(this);
            
            //State
            state = StateHelper.StateFromEnum(stateBehaviour, this);
            // Debug.Log(stateBehaviour);
            // Debug.Log(state);
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
            Destroy(gameObject, 0.1f);
        }
    }
}