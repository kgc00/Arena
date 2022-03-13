using UnityEngine;
using UnityEngine.Video;

namespace Utils {
    [RequireComponent(typeof(VideoPlayer))]
    public class VideoLoader : MonoBehaviour {
        [SerializeField] private string _filename;
        public VideoPlayer VideoPlayer { get; private set; }

        void Start() {
            VideoPlayer = GetComponent<VideoPlayer>();
            var fp = System.IO.Path.Combine(Application.streamingAssetsPath, _filename);
            VideoPlayer.url = fp;
            VideoPlayer.Prepare();
            VideoPlayer.Play();
        }
    }
}