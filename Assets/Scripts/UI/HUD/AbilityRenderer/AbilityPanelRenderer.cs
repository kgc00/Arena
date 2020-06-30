using System.Collections;
using System.Linq;
using Abilities;
using Enums;
using Players;
using UnityEngine;

namespace UI.HUD {
    public class AbilityPanelRenderer : MonoBehaviour {
        
        private Player localPlayer;
        [SerializeField] private GameObject AbilityPanel;
        
        private void OnEnable() {
            StartCoroutine(Initialize());
        }

        IEnumerator Initialize() {
            localPlayer = FindObjectsOfType<Player>()?.Where(x => x.ControlType == ControlType.Local).FirstOrDefault();
            
            yield return new WaitUntil(() => localPlayer?.Units[0]?.AbilityComponent?.initialized == true);
            
            var abilityComponent = localPlayer.Units[0].AbilityComponent;
            var equippedAbilities = abilityComponent.equippedAbilities;
            
            foreach (var kvp in equippedAbilities) {
                _ = Instantiate(AbilityPanel, gameObject.transform)
                    .GetComponent<AbilityRenderer>()
                    .Initialize(kvp);
            }
        }
    }
}