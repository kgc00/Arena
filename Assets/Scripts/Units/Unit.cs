using System;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Components;
using Controls;
using Data.Types;
using Data.UnitData;
using JetBrains.Annotations;
using State;
using UnityEngine;
using Utils;
using Players;
using Status;
using UI.Targeting;
using UnityEngine.Serialization;

namespace Units
{
    public class Unit : MonoBehaviour, IDamageable, IAbilityUser, IExperienceUser
    {
        public static Action<Unit> OnDeath = delegate {  };
        public List<Renderer> Renderers { get; protected set; }
        public Player Owner { get; private set; }
        [FormerlySerializedAs("type")] public UnitType unitType;
        public Sprite Portrait { get; protected set; }
        public TargetingUIController UIController { get; protected set; }
        [SerializeField] public Rigidbody Rigidbody { get; private set; }
        [SerializeField] public Animator Animator { get; private set; }
        private UnitState state;
        public Controller Controller { get; private set; }
        public InputModifierComponent InputModifierComponent { get; private set; }
        public AbilityComponent AbilityComponent { get; private set; }
        public HealthComponent HealthComponent { get; private set; }
        public ExperienceComponent ExperienceComponent { get; private set; }
        public StatusComponent StatusComponent { get; private set; }
        public CoroutineHelper CoroutineHelper { get; private set; }
        public StatsComponent StatsComponent { get; private set; }
        public void OnLevelUp() { }
        public bool Initialized { get; private set; } = false;

        public Unit Initialize (Player owner, UnitData data) {
            // properties & fields
            Owner = owner;
            Portrait = data.visualAssets.portrait;
            Renderers = GetComponentsInChildren<Renderer>().ToList();
            
            // Controller
            if (Controller == null) Controller = GetComponentInChildren<Controller>().Initialize(this);
            
            // Input Modifiers
            if (InputModifierComponent == null)
                InputModifierComponent = gameObject.AddComponent<InputModifierComponent>().Initialize(this);
            
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

            if (StatsComponent == null) StatsComponent = gameObject.AddComponent<StatsComponent>().Initialize(this, data.statsData);

            // if (UIController == null) UIController = gameObject.AddComponent<TargetingUIController>.Initialize(this, data.statsData);
                                                     
            // State
            state = StateHelper.StateFromEnum(data.state, this);
            
            Initialized = true;
            state.Enter ();
            return this;
        }

        public Unit UpdateModel(UnitData data) {
            Debug.Assert(Initialized);
            AbilityComponent.UpdateModel(data.abilities);
            HealthComponent.UpdateModel(data.health);
            StatsComponent.UpdateModel(data.statsData);
            return this;
        }
        
        void Update () {
            if (!Initialized) return;
            
            // handle state
            var newState = state?.HandleUpdate (Controller.InputValues);
            if (newState == null) return;
            state.Exit();
            state = newState;
            state.Enter();
        }

        private void FixedUpdate()
        {
            if (!Initialized) return;
            state?.HandleFixedUpdate(Controller.InputValues);
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

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            state?.HandleDrawGizmos();
        }

        private void OnGUI() {
            if (!Initialized) return;
            
            if (Owner.ControlType == ControlType.Local) {
                // GUILayout.Box($"State: {state}");
                // GUILayout.Box($"AbilityComponent State: {AbilityComponent.State}");
                // GUILayout.Box($"Input Values Forward: {Controller.InputValues.Forward}");
                // GUILayout.Box($"Input Values Horizontal: {Controller.InputValues.Horizontal}");
                // GUILayout.Box(transform.forward.ToString());
            }
            
            // if (Owner.ControlType == ControlType.Ai) {
            //     var width = 300;
            //     GUILayout.BeginArea(new Rect(Screen.width - width, 0, width, 60));
            //     GUILayout.Box($"State: {state}");
            //     GUILayout.EndArea();
            // }
        }
#endif
    }
}