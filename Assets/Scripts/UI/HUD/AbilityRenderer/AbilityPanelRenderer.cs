using System.Collections;
using System.Linq;
using Abilities;
using Data.Types;
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
            
            yield return new WaitUntil(() => {
                if (localPlayer == null) return false;
                if (localPlayer.Units.Count == 0) return false;

                var ac = localPlayer.Units[0].AbilityComponent;
                if(ac == null) return false;
                if (ac.State == AbilityComponentState.Idle) return true;
                
                return false;
            });
            
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