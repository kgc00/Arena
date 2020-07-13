using System;
using UnityEngine;

namespace Common {
    public class PanelGroup : MonoBehaviour {
        [SerializeField] private GameObject[] panels;
        // [SerializeField] private GameObject[] tabgroup;
        private int panelIndex;

        private void Awake() {
            ShowCurrentPanel();
        }

        private void ShowCurrentPanel() {
            for (int i = 0; i < panels.Length; i++) {
                if (i != panelIndex) panels[i].SetActive(false);
                else panels[i].SetActive(true);
            }
        }

        private void SetPageIndex(int index) {
            panelIndex = index;
            ShowCurrentPanel();
        }
    }
}