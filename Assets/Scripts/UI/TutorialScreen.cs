using UnityEngine;

namespace UI {
    public class TutorialScreen : MonoBehaviour {
        [SerializeField] private GameObject tutorialScreens;
        [SerializeField] private GameObject PreviousChevron;
        [SerializeField] private GameObject NextChevron;
        private int _numberOfScreens;
        private int _activeScreen;

        private void OnEnable() {
            _numberOfScreens = tutorialScreens.transform.childCount;
            _activeScreen = 1;
            HandlePreviousScreen();
        }

        public void HandleNextScreen() {
            if (_activeScreen == _numberOfScreens - 1) {
                NextChevron.SetActive(false);
                return;
            }
            PreviousChevron.SetActive(true);
            UpdateActiveScreen(_activeScreen + 1);
            if (_activeScreen == _numberOfScreens - 1) {
                NextChevron.SetActive(false);
            }
        }
        
        public void HandlePreviousScreen() { 
            if (_activeScreen == 0) {
                PreviousChevron.SetActive(false);
                return;
            }
            NextChevron.SetActive(true);
            UpdateActiveScreen(_activeScreen - 1);
            if (_activeScreen == 0) {
                PreviousChevron.SetActive(false);
            }
        }
        
        private void UpdateActiveScreen(int activeScreen) {
            for (int i = 0; i < _numberOfScreens; i++) {
                tutorialScreens.transform.GetChild(i).gameObject.SetActive(i == activeScreen);
            }
            _activeScreen = activeScreen;
        }
    }
}