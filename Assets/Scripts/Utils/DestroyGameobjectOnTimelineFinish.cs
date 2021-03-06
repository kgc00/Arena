﻿using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Utils {
    [RequireComponent(typeof(PlayableDirector))]
    public class DestroyGameobjectOnTimelineFinish : MonoBehaviour {
        private PlayableDirector _playableDirector;
        // todo, allow for resizing duration of control tracks 

        private void Start() {
            _playableDirector = GetComponent<PlayableDirector>();
            _playableDirector.stopped += HandleStopped;
        }

        private void HandleStopped(PlayableDirector obj) {
            Destroy(gameObject);
        }
    }
}