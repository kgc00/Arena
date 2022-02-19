using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Data.Types;
using Players;
using UnityEngine;
using Utils.NotificationCenter;

namespace UI.HUD.AbilityRenderer {
    public class AbilityPanelRenderer : MonoBehaviour {
        private Player _localPlayer;
        [SerializeField] private GameObject AbilityPanel;
        private List<AbilityRenderer> _renderers;
        private AbilityComponent _localUnitAbilityComponent;

        private void Start() {
            StartCoroutine(Initialize());
            this.AddObserver(HandleUnitModelUpdated, NotificationType.ComponentsDidUpdate);
        }

        private void OnDestroy() {
            this.RemoveObserver(HandleUnitModelUpdated, NotificationType.ComponentsDidUpdate);
        }
        
        IEnumerator Initialize() {
            _renderers = new List<AbilityRenderer>();
            _localPlayer = FindObjectsOfType<Player>()?.Where(x => x.ControlType == ControlType.Local).FirstOrDefault();

            yield return new WaitUntil(() => {
                if (_localPlayer == null) return false;
                if (_localPlayer.Units.Count == 0) return false;

                var ac = _localPlayer.Units[0].AbilityComponent;
                if (ac == null) return false;
                return ac.State == AbilityComponentState.Idle;
            });

            Debug.Assert(_localPlayer != null, nameof(_localPlayer) + " != null");
            _localUnitAbilityComponent = _localPlayer.Units[0].AbilityComponent;
            var equippedAbilities = _localUnitAbilityComponent.equippedAbilitiesByButton;

            foreach (var kvp in equippedAbilities) {
                var renderer = Instantiate(AbilityPanel, gameObject.transform)
                    .GetComponent<HUD.AbilityRenderer.AbilityRenderer>()
                    .Initialize(kvp);
                _renderers.Add(renderer);
            }
        }

        private void HandleUnitModelUpdated(object sender, object args) {
            // todo - validate on sender
            UpdateRendererAbilityReferences();
        }

        public void UpdateRendererAbilityReferences() {
            var equippedAbilities = _localUnitAbilityComponent.equippedAbilitiesByButton;

            foreach (var abilityRenderer in _renderers) {
                abilityRenderer.UpdateAbility(equippedAbilities.First(x => x.Value.Type == abilityRenderer.Ability.Type)
                    .Value);
            }
        }
    }
}