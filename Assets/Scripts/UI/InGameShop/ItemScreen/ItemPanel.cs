using Data.Items;
using Data.Types;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.UI;
using Utils.NotificationCenter;

namespace UI.InGameShop.ItemScreen {
    public class ItemPanel : MonoBehaviour {
        [SerializeField] private Material canPuchaseCostDifferenceMaterial;
        [SerializeField] private Material cannotPuchaseCostDifferenceMaterial;
        [SerializeField] public ItemData model;
        [SerializeField] public TextMeshProUGUI descriptionText;
        [SerializeField] public TextMeshProUGUI headerText;
        [SerializeField] public GameObject canPurchaseTextObject;
        [SerializeField] public TextMeshProUGUI costPriceText;
        [SerializeField] public TextMeshProUGUI costDifferenceText;
        [SerializeField] public GameObject hasPurchasedTextObject;
        [SerializeField] public Image itemImage;
        [SerializeField] public ItemScreen ItemScreen;
        [SerializeField] public GameObject purchaseButton;
        private Unit _purchasingUnit;
        private bool _isPurchased;
        public bool _shouldUpdateTextInEditor;

        private void OnValidate() {
            if (_shouldUpdateTextInEditor) {
                AssignText();
            }
        }

        private void AssignText() {
            if (model == null) return;

            if (descriptionText != null) {
                descriptionText.text = model.Description;
            }

            if (headerText != null) {
                headerText.text = model.Name;
            }

            if (costPriceText != null) {
                costPriceText.text = model.Cost.ToString();
            }

            if (itemImage != null) {
                itemImage.sprite = model.ItemImage;
            }

            if (_purchasingUnit != null && costDifferenceText != null && canPuchaseCostDifferenceMaterial != null &&
                cannotPuchaseCostDifferenceMaterial != null
                && hasPurchasedTextObject != null && canPurchaseTextObject != null) {
                // todo replace this with a real check
                if (!_isPurchased) {
                    var (containsEnoughFunds, remainder) =
                        _purchasingUnit.FundsComponent.ContainsEnoughFunds(model.Cost);
                    if (containsEnoughFunds) {
                        costDifferenceText.fontMaterial = canPuchaseCostDifferenceMaterial;
                        costDifferenceText.SetText($"(+{remainder})");
                    }
                    else {
                        costDifferenceText.fontMaterial = cannotPuchaseCostDifferenceMaterial;
                        costDifferenceText.SetText($"(-{remainder})");
                    }

                    hasPurchasedTextObject.SetActive(false);
                    canPurchaseTextObject.SetActive(true);
                }
                else {
                    canPurchaseTextObject.SetActive(false);
                    hasPurchasedTextObject.SetActive(true);
                }
            }

            if (purchaseButton != null) {
                purchaseButton.SetActive(!_isPurchased);
            }
        }

        private void OnEnable() {
            _purchasingUnit = InGameShopManager.Instance.PurchasingUnit;
            _isPurchased = _purchasingUnit.PurchasedItems.Contains(model.ItemType);
            AssignText();
        }

        public void HandlePurchase() {
            ItemScreen.HandlePurchase(model);
            _isPurchased = _purchasingUnit.PurchasedItems.Contains(model.ItemType);
            AssignText();
        }
    }
}