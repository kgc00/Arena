using Common;
using UnityEngine;

namespace Utils {
    public class CursorManager : Singleton<CursorManager> {
        [SerializeField] private Texture2D cursorTexture;

        private void Start() {
            var hotspot = new Vector2(cursorTexture.width * .5f, cursorTexture.height * .5f);
            Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
        }
    }
}