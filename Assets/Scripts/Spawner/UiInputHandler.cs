using System;
using UnityEngine;

namespace Spawner {
    public class UiInputHandler : MonoBehaviour {
        [SerializeField] private GameObject TopLevelControls;
        [SerializeField] private GameObject ChallengesScreen;
        [SerializeField] private GameObject UpgradesScreen;

        private void Start() {
            HandleBackButton();
        }

        public void HandleChallengesButton() {
            TopLevelControls.SetActive(false);
            ChallengesScreen.SetActive(true);
        }

        public void HandleUpgradesButton() {
            TopLevelControls.SetActive(false);
            UpgradesScreen.SetActive(true);
        }
        public void HandleBackButton(){
            ChallengesScreen.SetActive(false);            
            UpgradesScreen.SetActive(false);
            TopLevelControls.SetActive(true);
        }
        
    }
}