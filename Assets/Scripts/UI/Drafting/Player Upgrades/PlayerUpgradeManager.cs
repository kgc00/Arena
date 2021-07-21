using System;
using System.Collections.Generic;
using System.Linq;
using Abilities.Modifiers;
using Common;
using Common.Levels;
using Data;
using Data.Modifiers;
using Data.SpawnData;
using Data.Types;
using Data.UnitData;
using TypeReferences;
using UnityEngine;

namespace UI.Drafting.Player_Upgrades {
    public class PlayerUpgradeManager : MonoBehaviour,
        IModifierHandler<UnitSpawnData, UnitModifier>,
        IModifierHandler<UnitSpawnData, AbilityModifier> {
        public PlayerUpgradeManager Owner { get; private set; }
        [SerializeField] private UnitSpawnData model;

        public UnitSpawnData Model {
            get => model;
            set => model = value;
        }

        [SerializeField] private StatsUpgradesList statsUpgradesList;
        // [SerializeField] private AbilitiesUpgradesList abilitiesUpgradesList;
        private void Start() => Initialize(Model, this);

        public PlayerUpgradeManager Initialize(UnitSpawnData m, PlayerUpgradeManager o) {
            Owner = o;
            Model = m ? m.CreateInstance() : PersistentData.Instance.CurrentHordeModel[ControlType.Local].Waves[0].wave[0].CreateInstance();
            statsUpgradesList.Initialize(Model, this);
            // abilitiesUpgradesList.Initialize(Model, this);
            statsUpgradesList.UpdateList();
            Initialized = true;
            return this;
        }

        public bool Initialized { get; private set; }

        public void AddModifier(UnitSpawnData model, UnitModifier modifier, int cost) {
            if(DoesExist(modifier, Model)) return;
            Model.modifiers.Add(modifier.GetType());
            PersistentData.Instance.currency -= cost;
        }

        public void RemoveModifier(UnitSpawnData model, UnitModifier modifier, int cost) {
    
            // Handles case where modifier was already added
            // and new instance of same type is being supplied by input
            var m = GetModifier(modifier, Model);
            if (m != null) {
                Model.modifiers.Remove(m);
                PersistentData.Instance.currency += cost;
            }
        }
        
        
        private bool DoesExist(UnitModifier mod, UnitSpawnData selectedUnit) =>
            selectedUnit.modifiers.Exists(m => m.Type == mod.GetType());
        
        private ClassTypeReference GetModifier(UnitModifier mod, UnitSpawnData selectedUnit) =>
            selectedUnit.modifiers.FirstOrDefault(m => m.Type == mod.GetType());

        public void HandleContinueClick() {
            var hsd = PersistentData.Instance.CurrentHordeModel[ControlType.Local];
            hsd.Waves[0].wave[0] = Model;
            PersistentData.Instance.UpdateHordeModel(ControlType.Local, hsd);
            LevelDirector.Instance.LoadDrafting();
        }

        public void AddModifier(UnitSpawnData model, AbilityModifier modifier, int cost) {
            throw new NotImplementedException();
        }

        public void RemoveModifier(UnitSpawnData model, AbilityModifier modifier, int cost) {
            throw new NotImplementedException();
        }
    }
}