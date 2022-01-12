using System.Collections;
using System.Linq;
using Data.Types;
using Players;
using UnityEngine;

namespace UI.HUD {
    public class PortraitPanelRenderer : MonoBehaviour {
                
        private Player localPlayer;
        [SerializeField] private GameObject PortraitPanel;
        
        private void OnEnable() {
            StartCoroutine(Initialize());
        }

        IEnumerator Initialize() {
            localPlayer = FindObjectsOfType<Player>()?.Where(x => x.ControlType == ControlType.Local).FirstOrDefault();

            if (localPlayer == null) yield break;
            
            yield return new WaitUntil(() => localPlayer.Units.Count > 0);
            
            foreach (var unit in localPlayer.Units) {
                _ = Instantiate(PortraitPanel, gameObject.transform)
                    .GetComponent<PortraitRenderer.PortraitRenderer>()
                    .Initialize(unit);
            }
        }
    }
}