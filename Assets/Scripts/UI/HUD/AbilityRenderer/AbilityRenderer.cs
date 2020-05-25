using System;
using System.Collections.Generic;
using Abilities;
using Controls;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI.HUD {
    public class AbilityRenderer : MonoBehaviour {
       [SerializeField] public GameObject timerGo;
       [SerializeField] public GameObject keyGo;
       [SerializeField] public GameObject iconGo;
       [SerializeField] public GameObject iconRadialFillGo;
       [HideInInspector]public VerticalLayoutGroup VerticalLayoutGroup;
       public TextMeshProUGUI key {get; set; }
       public TextMeshProUGUI timer {get; set; }
       public Image icon { get; set; }
       public Image iconRadialFill { get; set; }
       public Ability ability { get; private set; }
       private State state;
        public AbilityRenderer Initialize(KeyValuePair<ButtonType, Ability> kvp) {
            VerticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
            key = keyGo.GetComponent<TextMeshProUGUI>();
            timer = timerGo.GetComponent<TextMeshProUGUI>();
            icon = iconGo.GetComponent<Image>();
            iconRadialFill = iconRadialFillGo.GetComponent<Image>();
            ability = kvp.Value;
            
            key.SetText(NameMap(kvp.Key.ToString()));
            icon.sprite = kvp.Value.Icon;
            iconRadialFill.sprite = kvp.Value.Icon;
            
            state = new IdleState(this);
            
            return this;
        }

        string NameMap(string s) {
            if (s == ButtonType.Skill1.ToString()) return "Q";
            if (s == ButtonType.Skill2.ToString()) return "E";
            if (s == ButtonType.Skill3.ToString()) return "R";
            if (s == ButtonType.Skill4.ToString()) return "F";
            if (s == ButtonType.Normal1.ToString()) return "LMB";
            if (s == ButtonType.Normal2.ToString()) return "RMB";
            return "";
        }

        private void Update() {
            var newState = state?.HandleUpdate ();
            if (newState == null) return;
            state.Exit();
            state = newState;
            state.Enter();
        }
    }
}