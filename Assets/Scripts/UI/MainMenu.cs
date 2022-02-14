using Common.Levels;
using UnityEngine;

namespace UI {
    public class MainMenu : MonoBehaviour {
        [SerializeField] private GameObject MainMenuGameObject;
        [SerializeField] private GameObject TutorialGameObject;
        public void HandleStart() {
            LevelDirector.Instance.LoadArena();
        }

        public void HandleTutorial() {
            MainMenuGameObject.SetActive(false);
            TutorialGameObject.SetActive(true);
        }

        public void HandleReturn() {
            TutorialGameObject.SetActive(false);
            MainMenuGameObject.SetActive(true);
        }

        public void HandleExit() {
            Application.Quit();
        }
    }
}