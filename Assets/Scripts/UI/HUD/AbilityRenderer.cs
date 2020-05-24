using System;
using System.Collections.Generic;
using Abilities;
using Controls;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI.HUD {
    public class AbilityRenderer : MonoBehaviour {
       [SerializeField] private GameObject timerGo;
       [SerializeField] private GameObject keyGo;
       [SerializeField] private GameObject iconGo;
       private TextMeshProUGUI key;
       private TextMeshProUGUI timer;
       private Image icon;
       private Ability ability;
        public AbilityRenderer Initialize(KeyValuePair<ButtonType, Ability> kvp) {
            key = keyGo.GetComponent<TextMeshProUGUI>();
            timer = timerGo.GetComponent<TextMeshProUGUI>();
            icon = iconGo.GetComponent<Image>();
            ability = kvp.Value;
            
            key.SetText(NameMap(kvp.Key.ToString()));
            icon.sprite = kvp.Value.Icon;
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
            
            
            if (ability?.Cooldown?.IsOnCooldown == true) {
                Debug.Log("cd");
            }
        }
    }
}