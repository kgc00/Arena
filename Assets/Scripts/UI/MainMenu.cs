using System.Collections;
using Audio;
using Common.Levels;
using Data.Types;
using UnityEngine;
using Utils.NotificationCenter;

namespace UI {
    public class MainMenu : MonoBehaviour {
        [SerializeField] private GameObject MainMenuGameObject;
        [SerializeField] private GameObject TutorialGameObject;
        private Coroutine _startCRT;
        private bool isTransitioningScenes => _startCRT != null;

        private void Start() {
            AudioService.Instance.RequestBGM();
        }

        public void HandleStart() {
            if (!isTransitioningScenes) {
                _startCRT = StartCoroutine(StartCRT());
            }
        }

        private IEnumerator StartCRT() {
            this.PostNotification(NotificationType.DidStartGame);
            yield return new WaitForSeconds(2f);
            LevelDirector.Instance.LoadArena();
        }

        public void HandleTutorial() {
            if (isTransitioningScenes) return;
            this.PostNotification(NotificationType.DidClickShopButton);
            MainMenuGameObject.SetActive(false);
            TutorialGameObject.SetActive(true);
        }

        public void HandleReturn() {
            if (isTransitioningScenes) return;
            this.PostNotification(NotificationType.DidClickShopButton);
            TutorialGameObject.SetActive(false);
            MainMenuGameObject.SetActive(true);
        }

        public void HandleExit() {
            if (isTransitioningScenes) return;
            this.PostNotification(NotificationType.DidClickShopButton);
            Application.Quit();
        }
    }
}