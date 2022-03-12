using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using Data.Types;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Utils.NotificationCenter;

namespace Audio {
    public class AudioService : Singleton<AudioService> {
        [Header("Settings")] [SerializeField] private AudioMixer _mixer;

        [Header("BGM")] [SerializeField] private bool _playBGM;
        [SerializeField] private AudioClip _arenaBGM;
        [SerializeField] private AudioClip _menuBGM;

        [Header("Sfx")] [SerializeField] private AudioClip PurchaseComplete;
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
        [SerializeField] private AudioClip GameOver;
        [SerializeField] private AudioClip RainLoop;
        [SerializeField] private AudioClip WaveCleared;
        [SerializeField] private AudioClip InsufficientFundsForPurchase;
        [SerializeField] private AudioClip ClickIncrement;
        [SerializeField] private AudioClip ClickDecrement;
        [SerializeField] private AudioClip UISoftWarning;
        [SerializeField] private AudioClip DidClickCloseShopButton;
        [SerializeField] private AudioClip DidLose;
        [SerializeField] private AudioClip DidWin;
        [SerializeField] private AudioClip DidStartGame;

        private Dictionary<AudioSourceType, AudioSource> _audioSources;
        private Sequence _rainFadeOutSequence;
        private Sequence _bgmFadeOutSequence;

        private enum AudioSourceType {
            BGM,
            SFX,
            Rain
        }

        private void InitializeAudioSources() {
            var bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.outputAudioMixerGroup = _mixer.FindMatchingGroups(AudioSourceType.BGM.ToString()).First();
            var sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.outputAudioMixerGroup = _mixer.FindMatchingGroups(AudioSourceType.SFX.ToString()).First();
            var rainSource = gameObject.AddComponent<AudioSource>(); // non-PlayOneShot clips need their own source
            rainSource.outputAudioMixerGroup = _mixer.FindMatchingGroups(AudioSourceType.SFX.ToString()).First();

            _audioSources = new Dictionary<AudioSourceType, AudioSource> {
                {AudioSourceType.BGM, bgmSource},
                {AudioSourceType.SFX, sfxSource},
                {AudioSourceType.Rain, rainSource},
            };
        }

        public void PlaySFX(AudioClip clip) {
            var audioSource = _audioSources[AudioSourceType.SFX];
            audioSource.PlayOneShot(clip);
        }

        private void StartBGM() {
            var audioSource = _audioSources[AudioSourceType.BGM];
            Debug.Assert(audioSource.outputAudioMixerGroup != null);
            var nextClip = SceneManager.GetActiveScene().name == "Arena" ? _arenaBGM : _menuBGM;
            if (nextClip == audioSource.clip) return;
            audioSource.clip = nextClip;
            audioSource.loop = true;
            audioSource.Play();
        }

        public void RequestBGM() {
            if (_playBGM) StartBGM();
        }
        
        public void RequestFadeOutBGM() {
            if (_playBGM) {
                _bgmFadeOutSequence.Restart();
            }
        }

        private void PlayRain() {
            var audioSource = _audioSources[AudioSourceType.Rain];
            audioSource.volume = 0.8F;
            audioSource.clip = RainLoop;
            audioSource.loop = true;
            audioSource.Play();
        }

        private void StopPlayingRain() {
            if (_rainFadeOutSequence.IsPlaying()) return;

            _rainFadeOutSequence.Restart();
        }

        private void OnDisable() {
            Cleanup();
        }

        private void OnDestroy() {
            Cleanup();
        }

        private void OnEnable() {
            InitializeAudioSources();

            var rainAudioSource = _audioSources[AudioSourceType.Rain];
            _rainFadeOutSequence = DOTween.Sequence()
                .Append(rainAudioSource.DOFade(0, 0.5f)).AppendCallback(() => {
                    rainAudioSource.Stop();
                    rainAudioSource.time = 0;
                }).SetAutoKill(false);
            var bgmAudioSource = _audioSources[AudioSourceType.BGM];
            _bgmFadeOutSequence = DOTween.Sequence()
                .Append(bgmAudioSource.DOFade(0, 1f)).AppendCallback(() => {
                    bgmAudioSource.Stop();
                    bgmAudioSource.time = 0;
                    bgmAudioSource.volume = 1;
                }).SetAutoKill(false).Pause();

            if (_playBGM) StartBGM();
            SceneManager.activeSceneChanged += HandleSceneChanged;
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
            this.AddObserver(HandleRainDidFinish, NotificationType.RainDidFinish);
            this.AddObserver(HandleGameOver, NotificationType.GameOver);
            this.AddObserver(HandleWaveCleared, NotificationType.WaveCleared);
            this.AddObserver(HandleInsufficientFundsForPurchase, NotificationType.InsufficientFundsForPurchase);
            this.AddObserver(HandleClickIncrement, NotificationType.ClickIncrement);
            this.AddObserver(HandleClickDecrement, NotificationType.ClickDecrement);
            this.AddObserver(HandleUISoftWarning, NotificationType.UISoftWarning);
            this.AddObserver(HandleDidClickCloseShopButton, NotificationType.DidClickCloseShopButton);
            this.AddObserver(HandleDidWin, NotificationType.DidWin);
            this.AddObserver(HandleDidLose, NotificationType.DidLose);
            this.AddObserver(HandleDidStartGame, NotificationType.DidStartGame);
        }

        private void Cleanup() {
            _bgmFadeOutSequence?.Kill();
            _rainFadeOutSequence?.Kill();
            _audioSources?.Clear();
            SceneManager.activeSceneChanged -= HandleSceneChanged;
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
            this.RemoveObserver(HandleRainDidFinish, NotificationType.RainDidFinish);
            this.RemoveObserver(HandleGameOver, NotificationType.GameOver);
            this.RemoveObserver(HandleWaveCleared, NotificationType.WaveCleared);
            this.RemoveObserver(HandleInsufficientFundsForPurchase, NotificationType.InsufficientFundsForPurchase);
            this.RemoveObserver(HandleClickIncrement, NotificationType.ClickIncrement);
            this.RemoveObserver(HandleClickDecrement, NotificationType.ClickDecrement);
            this.RemoveObserver(HandleUISoftWarning, NotificationType.UISoftWarning);
            this.RemoveObserver(HandleDidClickCloseShopButton, NotificationType.DidClickCloseShopButton);
            this.RemoveObserver(HandleDidWin, NotificationType.DidWin);
            this.RemoveObserver(HandleDidLose, NotificationType.DidLose);
            this.RemoveObserver(HandleDidStartGame, NotificationType.DidStartGame);
        }
        private void HandleSceneChanged(Scene currentScene, Scene nextScene) {
            _audioSources[AudioSourceType.SFX]?.Stop();
        }

        private void HandleDidStartGame(object arg1, object arg2) {
            if (_playBGM) {
                _bgmFadeOutSequence.Restart();
            }

            PlaySFX(DidStartGame);
        }

        private void HandleDidLose(object arg1, object arg2) {
            PlaySFX(DidLose);
        }

        private void HandleDidWin(object arg1, object arg2) {
            PlaySFX(DidWin);
        }

        private void HandleDidClickCloseShopButton(object arg1, object arg2) {
            PlaySFX(DidClickCloseShopButton);
        }

        private void HandleUISoftWarning(object arg1, object arg2) {
            PlaySFX(UISoftWarning);
        }

        private void HandleClickIncrement(object arg1, object arg2) {
            PlaySFX(ClickIncrement);
        }

        private void HandleClickDecrement(object arg1, object arg2) {
            PlaySFX(ClickDecrement);
        }

        private void HandleInsufficientFundsForPurchase(object arg1, object arg2) {
            PlaySFX(InsufficientFundsForPurchase);
        }

        private void HandleWaveCleared(object arg1, object arg2) {
            PlaySFX(WaveCleared);
        }

        private void HandleRainDidFinish(object arg1, object arg2) {
            StopPlayingRain();
        }

        private void HandleGameOver(object arg1, object arg2) {
            if (_playBGM) {
                _bgmFadeOutSequence.Restart();
            }

            PlaySFX(GameOver);
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
            PlayRain();
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