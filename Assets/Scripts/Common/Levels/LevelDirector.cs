using UnityEngine.SceneManagement;

namespace Common.Levels {
    public class LevelDirector : Singleton<LevelDirector> {
        public void LoadArena() => SceneManager.LoadScene((int) LevelIndex.Arena); 
        public void LoadMain() => SceneManager.LoadScene((int) LevelIndex.Main);
        public void LoadWin() => SceneManager.LoadScene((int) LevelIndex.Win);
        public void LoadLose() => SceneManager.LoadScene((int) LevelIndex.Lose);
    }
}