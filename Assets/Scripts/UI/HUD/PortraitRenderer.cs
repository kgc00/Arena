using TMPro;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD {
    public class PortraitRenderer : MonoBehaviour {
        private Unit unit;
        [SerializeField]private GameObject nameGo;
        [SerializeField]private GameObject portraitGo;
        private Image portrait;
        private TextMeshProUGUI name;
        public PortraitRenderer Initialize(Unit unit) {
            this.unit = unit;
            name = nameGo.GetComponent<TextMeshProUGUI>();
            name.SetText(unit.name);
            portrait = portraitGo.GetComponent<Image>();
            portrait.sprite = unit.portrait;
            return this;
        }
    }
}