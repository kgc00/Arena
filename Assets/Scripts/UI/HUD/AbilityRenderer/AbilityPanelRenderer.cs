using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Data.Types;
using Players;
using Sirenix.Utilities;
using UI.InGameShop;
using UnityEngine;
using Utils.NotificationCenter;

namespace UI.HUD {
    public class AbilityPanelRenderer : MonoBehaviour {
        
        private Player _localPlayer;
        [SerializeField] private GameObject AbilityPanel;
        private List<AbilityRenderer.AbilityRenderer> _renderers;

        private void Start() {
            StartCoroutine(Initialize());
        }

        IEnumerator Initialize() {
            _renderers = new List<AbilityRenderer.AbilityRenderer>();
            _localPlayer = FindObjectsOfType<Player>()?.Where(x => x.ControlType == ControlType.Local).FirstOrDefault();
            
            yield return new WaitUntil(() => {
                if (_localPlayer == null) return false;
                if (_localPlayer.Units.Count == 0) return false;

                var ac = _localPlayer.Units[0].AbilityComponent;
                if(ac == null) return false;
                return ac.State == AbilityComponentState.Idle;
            });

            Debug.Assert(_localPlayer != null, nameof(_localPlayer) + " != null");
            var abilityComponent = _localPlayer.Units[0].AbilityComponent;
            var equippedAbilities = abilityComponent.equippedAbilitiesByButton;
            
            foreach (var kvp in equippedAbilities) {
               var renderer = Instantiate(AbilityPanel, gameObject.transform)
                    .GetComponent<AbilityRenderer.AbilityRenderer>()
                    .Initialize(kvp);
               _renderers.Add(renderer);
            }
        }
    }
}