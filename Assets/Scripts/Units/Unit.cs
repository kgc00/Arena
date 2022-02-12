using System;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Components;
using Controls;
using Data.Items;
using Data.Types;
using Data.UnitData;
using State;
using UnityEngine;
using Utils;
using Players;
using Status;
using UI.Targeting;
using UnityEngine.Serialization;

namespace Units
{
    public partial class Unit : MonoBehaviour, IDamageable, IAbilityUser, IExperienceUser {
        public List<ItemType> PurchasedItems;
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
        public FundsComponent FundsComponent { get; private set; }
        public UnitData UnitData;
        public bool Initialized { get; private set; } = false;

        private void Awake() {
            PurchasedItems = new List<ItemType>();
        }

        public Unit Initialize (Player owner, UnitData data) {
            // properties & fields
            UnitData = data;
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

            // Stats -- must occur before abilities & health
            if (StatsComponent == null) StatsComponent = gameObject.AddComponent<StatsComponent>().Initialize(this, data.statsData);
            
            // Health
            if (HealthComponent == null) HealthComponent = gameObject.AddComponent<HealthComponent>().Initialize(this, data.health, StatsComponent);

            // Abilities
            if (AbilityComponent == null) AbilityComponent= gameObject.AddComponent<AbilityComponent>().Initialize(this, data.abilities, StatsComponent); 
            
            // Experience
            if (ExperienceComponent == null) ExperienceComponent = gameObject.AddComponent<ExperienceComponent>().Initialize(this, data.experience);
            
            // CoroutineHelper
            if (CoroutineHelper == null) CoroutineHelper = gameObject.AddComponent<CoroutineHelper>().Initialize(this);
            
            // Status 
            if (StatusComponent == null) StatusComponent = gameObject.AddComponent<StatusComponent>().Initialize(this);

            // Funds
            if (FundsComponent == null) FundsComponent = gameObject.AddComponent<FundsComponent>().Initialize(this, data.fundsData);

            if (UIController == null) UIController = gameObject.AddComponent<TargetingUIController>().Initialize(this);
                                                     
            // State
            state = StateHelper.StateFromEnum(data.state, this);
            
            Initialized = true;
            state.Enter ();
            return this;
        }
        
        public Unit UpdateComponents() {
            Debug.Assert(Initialized);
            // stats component not updated to keep gains from level ups
            AbilityComponent.UpdateModel(UnitData.abilities);
            HealthComponent.UpdateModel(UnitData.health, StatsComponent);
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

        public virtual void OnLevelUp() {
            var spawnPos = new Vector3(transform.position.x, 0, transform.position.z);
            MonoHelper.SpawnVfx(VfxType.LevelUp, spawnPos).transform.SetParent(transform);
            StatsComponent.IncrementStat(StatType.Agility, 2);
            StatsComponent.IncrementStat(StatType.Endurance, 2);
            StatsComponent.IncrementStat(StatType.Intelligence, 2);
            StatsComponent.IncrementStat(StatType.Strength, 2);
            StatsComponent.IncrementStat(StatType.MovementSpeed, 2);
            UpdateComponents();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            state?.HandleDrawGizmos();
        }

        private void OnGUI() {
            if (!Initialized) return;
            
            if (Owner.ControlType == ControlType.Local) {
                if (GUILayout.Button("Add Exp"))
                {
                    ExperienceComponent.AwardBounty(10);
                }
                GUILayout.Box($"stats: {StatsComponent.Stats.Intelligence.Value.ToString()}");
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
            
            state?.HandleOnGUI();
        }
#endif
    }
}