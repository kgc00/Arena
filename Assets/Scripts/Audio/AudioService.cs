using System;
using System.Collections.Generic;
using System.Linq;
using Data.Types;
using UnityEngine;
using UnityEngine.Audio;
using Utils.NotificationCenter;

namespace Audio {
    public class AudioService : MonoBehaviour {
        [Header("Settings")] [SerializeField] private AudioMixer _mixer;

        [Header("BGM")] [SerializeField] private bool _playBGM;
        [SerializeField] private AudioClip _bgm;

        [Header("Sfx")]
        [SerializeField] private AudioClip PurchaseComplete;
        [SerializeField] private AudioClip DidCastBurst;
        [SerializeField] private AudioClip DidCastPierceAndPull;
        [SerializeField] private AudioClip DidLaunchPierceAndPull;
        [SerializeField] private AudioClip DidCastConceal;
        [SerializeField] private AudioClip DidCastPrey;
        [SerializeField] private AudioClip DidCastMark;
        [SerializeField] private AudioClip DidCastRain;
        [SerializeField] private AudioClip DidConnectBurst;
        [SerializeField] private AudioClip DidConnectPierceAndPull;
        [SerializeField] private AudioClip DidConnectPrey;
        [SerializeField] private AudioClip DidConnectMark;
        [SerializeField] private AudioClip DidApplyMark;
        [SerializeField] private AudioClip DidTriggerMark;
        [SerializeField] private AudioClip DidConnectCharge;
        [SerializeField] private AudioClip DidCastChainFlame;
        [SerializeField] private AudioClip DidConnectChainFlame;
        [SerializeField] private AudioClip DidCastIceBolt;
        [SerializeField] private AudioClip DidConnectIceBolt;
        [SerializeField] private AudioClip DidCastRoar;
        [SerializeField] private AudioClip DidConnectRoar;
        [SerializeField] private AudioClip DidToggleShopTab;
        [SerializeField] private AudioClip DidClickShopButton;
        [SerializeField] private AudioClip AttackDidCollide;
        [SerializeField] private AudioClip DidPickupHealth;
        [SerializeField] private AudioClip DidLevelUp;
        
        private Dictionary<AudioSourceType, AudioSource> _audioSources;

        private enum AudioSourceType {
            BGM,
            SFX
        }

        private void InitializeAudioSources() {
            var bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.outputAudioMixerGroup = _mixer.FindMatchingGroups(AudioSourceType.BGM.ToString()).First();
            var sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.outputAudioMixerGroup = _mixer.FindMatchingGroups(AudioSourceType.SFX.ToString()).First();

            _audioSources = new Dictionary<AudioSourceType, AudioSource> {
                {AudioSourceType.BGM, bgmSource},
                {AudioSourceType.SFX, sfxSource}
            };
        }

        public void PlaySFX(AudioClip clip) {
            var audioSource = _audioSources[AudioSourceType.SFX];
            if (audioSource == null) {
                Debug.Log("null");
            }
            audioSource.PlayOneShot(clip);
        }

        private void StartBGM() {
            var audioSource = _audioSources[AudioSourceType.BGM];
            Debug.Assert(audioSource.outputAudioMixerGroup != null);
            audioSource.clip = _bgm;
            audioSource.loop = true;
            audioSource.Play();
        }
        
        private void OnDestroy() {
            _audioSources.Clear();
            this.RemoveObserver(HandleDidLevelUp, NotificationType.DidLevelUp);
            this.RemoveObserver(HandleAttackDidCollide, NotificationType.AttackDidCollide);
            this.RemoveObserver(HandleDidPickupHealth, NotificationType.DidPickupHealth);
            this.RemoveObserver(HandleDidLaunchPierceAndPull, NotificationType.DidLaunchPierceAndPull);
            this.RemoveObserver(HandlePurchase, NotificationType.PurchaseComplete);
            this.RemoveObserver(HandleDidCastBurst, NotificationType.DidCastBurst);
            this.RemoveObserver(HandleDidCastPierceAndPull, NotificationType.DidCastPierceAndPull);
            this.RemoveObserver(HandleDidCastConceal, NotificationType.DidCastConceal);
            this.RemoveObserver(HandleDidCastPrey, NotificationType.DidCastPrey);
            this.RemoveObserver(HandleDidCastMark, NotificationType.DidCastMark);
            this.RemoveObserver(HandleDidCastRain, NotificationType.DidCastRain);
            this.RemoveObserver(HandleDidConnectBurst, NotificationType.DidConnectBurst);
            this.RemoveObserver(HandleDidConnectPierceAndPull, NotificationType.DidConnectPierceAndPull);
            this.RemoveObserver(HandleDidConnectPrey, NotificationType.DidConnectPrey);
            this.RemoveObserver(HandleDidConnectMark, NotificationType.DidConnectMark);
            this.RemoveObserver(HandleDidApplyMark, NotificationType.DidApplyMark);
            this.RemoveObserver(HandleDidTriggerMark, NotificationType.DidTriggerMark);
            this.RemoveObserver(HandleDidConnectCharge, NotificationType.DidConnectCharge);
            this.RemoveObserver(HandleDidCastChainFlame, NotificationType.DidCastChainFlame);
            this.RemoveObserver(HandleDidConnectChainFlame, NotificationType.DidConnectChainFlame);
            this.RemoveObserver(HandleDidCastIceBolt, NotificationType.DidCastIceBolt);
            this.RemoveObserver(HandleDidConnectIceBolt, NotificationType.DidConnectIceBolt);
            this.RemoveObserver(HandleDidCastRoar, NotificationType.DidCastRoar);
            this.RemoveObserver(HandleDidConnectRoar, NotificationType.DidConnectRoar);
            this.RemoveObserver(HandleDidToggleShopTab, NotificationType.DidToggleShopTab);
            this.RemoveObserver(HandleDidClickShopButton, NotificationType.DidClickShopButton);
        }
        
        private void Start() {
            InitializeAudioSources();
            if (_playBGM) StartBGM();
            this.AddObserver(HandleDidLevelUp, NotificationType.DidLevelUp);
            this.AddObserver(HandleAttackDidCollide, NotificationType.AttackDidCollide);
            this.AddObserver(HandleDidPickupHealth, NotificationType.DidPickupHealth);
            this.AddObserver(HandleDidLaunchPierceAndPull, NotificationType.DidLaunchPierceAndPull);
            this.AddObserver(HandlePurchase, NotificationType.PurchaseComplete);
            this.AddObserver(HandleDidCastBurst, NotificationType.DidCastBurst);
            this.AddObserver(HandleDidCastPierceAndPull, NotificationType.DidCastPierceAndPull);
            this.AddObserver(HandleDidCastConceal, NotificationType.DidCastConceal);
            this.AddObserver(HandleDidCastPrey, NotificationType.DidCastPrey);
            this.AddObserver(HandleDidCastMark, NotificationType.DidCastMark);
            this.AddObserver(HandleDidCastRain, NotificationType.DidCastRain);
            this.AddObserver(HandleDidConnectBurst, NotificationType.DidConnectBurst);
            this.AddObserver(HandleDidConnectPierceAndPull, NotificationType.DidConnectPierceAndPull);
            this.AddObserver(HandleDidConnectPrey, NotificationType.DidConnectPrey);
            this.AddObserver(HandleDidConnectMark, NotificationType.DidConnectMark);
            this.AddObserver(HandleDidApplyMark, NotificationType.DidApplyMark);
            this.AddObserver(HandleDidTriggerMark, NotificationType.DidTriggerMark);
            this.AddObserver(HandleDidConnectCharge, NotificationType.DidConnectCharge);
            this.AddObserver(HandleDidCastChainFlame, NotificationType.DidCastChainFlame);
            this.AddObserver(HandleDidConnectChainFlame, NotificationType.DidConnectChainFlame);
            this.AddObserver(HandleDidCastIceBolt, NotificationType.DidCastIceBolt);
            this.AddObserver(HandleDidConnectIceBolt, NotificationType.DidConnectIceBolt);
            this.AddObserver(HandleDidCastRoar, NotificationType.DidCastRoar);
            this.AddObserver(HandleDidConnectRoar, NotificationType.DidConnectRoar);
            this.AddObserver(HandleDidToggleShopTab, NotificationType.DidToggleShopTab);
            this.AddObserver(HandleDidClickShopButton, NotificationType.DidClickShopButton);
        }

        private void HandleDidLevelUp(object arg1, object arg2) {
            PlaySFX(DidLevelUp);
        }

        private void HandleDidPickupHealth(object arg1, object arg2) {
            PlaySFX(DidPickupHealth);
        }

        private void HandleAttackDidCollide(object arg1, object arg2) {
            PlaySFX(AttackDidCollide);
        }

        private void HandleDidLaunchPierceAndPull(object arg1, object arg2) {
            PlaySFX(DidLaunchPierceAndPull);
        }

        private void HandlePurchase(object arg1, object arg2) {
            PlaySFX(PurchaseComplete);
        }
        
        private void HandleDidClickShopButton(object arg1, object arg2) {
            PlaySFX(DidClickShopButton);
        }

        private void HandleDidToggleShopTab(object arg1, object arg2) {
            PlaySFX(DidToggleShopTab);
        }

        private void HandleDidConnectRoar(object arg1, object arg2) {
            PlaySFX(DidConnectRoar);
        }

        private void HandleDidCastRoar(object arg1, object arg2) {
            PlaySFX(DidCastRoar);
        }

        private void HandleDidConnectIceBolt(object arg1, object arg2) {
            PlaySFX(DidConnectIceBolt);
        }

        private void HandleDidCastIceBolt(object arg1, object arg2) {
            PlaySFX(DidCastIceBolt);
        }

        private void HandleDidConnectChainFlame(object arg1, object arg2) {
            PlaySFX(DidConnectChainFlame);
        }

        private void HandleDidCastChainFlame(object arg1, object arg2) {
            PlaySFX(DidCastChainFlame);
        }

        private void HandleDidConnectCharge(object arg1, object arg2) {
            PlaySFX(DidConnectCharge);
        }

        private void HandleDidTriggerMark(object arg1, object arg2) {
            PlaySFX(DidTriggerMark);
        }

        private void HandleDidConnectMark(object arg1, object arg2) {
            PlaySFX(DidConnectMark);
        }

        private void HandleDidConnectPrey(object arg1, object arg2) {
            PlaySFX(DidConnectPrey);
        }

        private void HandleDidConnectPierceAndPull(object arg1, object arg2) {
            PlaySFX(DidConnectPierceAndPull);
        }

        private void HandleDidConnectBurst(object arg1, object arg2) {
            PlaySFX(DidConnectBurst);
        }

        private void HandleDidCastRain(object arg1, object arg2) {
            PlaySFX(DidCastRain);
        }

        private void HandleDidCastMark(object arg1, object arg2) {
            PlaySFX(DidCastMark);
        }

        private void HandleDidCastPrey(object arg1, object arg2) {
            PlaySFX(DidCastPrey);
        }

        private void HandleDidCastConceal(object arg1, object arg2) {
            PlaySFX(DidCastConceal);
        }

        private void HandleDidCastPierceAndPull(object arg1, object arg2) {
            PlaySFX(DidCastPierceAndPull);
        }

        private void HandleDidCastBurst(object arg1, object arg2) {
            PlaySFX(DidCastBurst);
        }

        private void HandleDidApplyMark(object arg1, object arg2) {
            PlaySFX(DidApplyMark);
        }
    }
}