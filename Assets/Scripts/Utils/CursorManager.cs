using Common;
using UnityEngine;

namespace Utils {
    public class CursorManager : Singleton<CursorManager> {
        [SerializeField] private Texture2D cursorTexture;
        private void Start() {
            Cursor.SetCursor(cursorTexture, new Vector2(0,0), CursorMode.Auto);
        }
    }
}