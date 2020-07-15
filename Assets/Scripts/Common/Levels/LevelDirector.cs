using Levels;
using UnityEngine.SceneManagement;

namespace Common.Levels {
    public class LevelDirector : Singleton<LevelDirector> {
        public void LoadArena() => SceneManager.LoadScene((int) LevelIndex.Arena);

        public void LoadDrafting() => SceneManager.LoadScene((int) LevelIndex.Drafting);
    }
}