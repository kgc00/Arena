
using System;
using System.Collections.Generic;
using System.Linq;
using Data.Types;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using Utils.NotificationCenter;

namespace UI.InGameShop.StatScreen {
    public class StatScreen : ShopScreen {
        [SerializeField] private TextMeshProUGUI _availableSkillPointsText;
        private Dictionary<StatType, StatPanel> _panels;
        private InGameShopManager _inGameShopManager;
        public int SkillPointBank { get; private set; }

        private void OnEnable() {
            if (_inGameShopManager == null) {
                _inGameShopManager = FindObjectOfType<InGameShopManager>();
            }
            var statPanels = GetComponentsInChildren<StatPanel>();
            if (statPanels != null) {
                _panels = statPanels.ToDictionary(x => x.StatType);
            }
            else {
                throw new Exception("No panels in children");
            }

            UpdateSkillBank();
        }

        private void UpdateSkillBank() {
            var purchasingUnit = _inGameShopManager.PurchasingUnit;
            Debug.Assert(purchasingUnit != null);
            SkillPointBank = purchasingUnit ? purchasingUnit.ExperienceComponent.SkillPoints : 0;
            _availableSkillPointsText.text = SkillPointBank.ToString();
        }

        public void IncrementSkillBank() {
            SkillPointBank++;
            _availableSkillPointsText.text = SkillPointBank.ToString();
        }
        public void DecrementSkillBank() {
            SkillPointBank--;
            _availableSkillPointsText.text = SkillPointBank.ToString();
        }

        public void HandlePurchase() {
            var purchasingUnit = _inGameShopManager.PurchasingUnit;
            Debug.Assert(purchasingUnit != null);
            purchasingUnit.ExperienceComponent.SkillPoints = SkillPointBank;
            _panels.ForEach(x => x.Value.HandlePurchase(purchasingUnit));
            purchasingUnit.UpdateComponents();
            this.PostNotification(NotificationType.PurchaseComplete);
        }
    }
}