using System.Collections;
using Audio;
using Common.Levels;
using Data.Types;
using UnityEngine;
using UnityEngine.UI;
using Utils.NotificationCenter;

namespace UI {
    public class MainMenu : MonoBehaviour {
        [SerializeField] private GameObject MainMenuGameObject;
        [SerializeField] private GameObject TutorialGameObject;
        [SerializeField] private Toggle _soundToggle;
        [SerializeField] private Image _soundToggleImage;
        [SerializeField] private Sprite _soundToggleImageOn;
        [SerializeField] private Sprite _soundToggleImageOff;
        private Coroutine _startCRT;
        private bool isTransitioningScenes => _startCRT != null;

        private void Start() {
            _soundToggle.isOn = !AudioService.Instance.IsSoundDisabled;
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

        public void HandleSoundToggle() {
            if (_soundToggle.isOn) {
                _soundToggleImage.sprite = _soundToggleImageOn;
                AudioService.Instance.SetSoundEnabled(true);
            }
            else {
                _soundToggleImage.sprite = _soundToggleImageOff;
                AudioService.Instance.SetSoundEnabled(false);
            }
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