using System;
using Units;
using UnityEngine;

namespace UI.Targeting
{
    public class TargetingTest : MonoBehaviour
    {
        public Material Material;
        public Transform playerTransform;

        private void Awake()
        {
            Material = GetComponent<Renderer>().material;
            playerTransform = FindObjectOfType<Unit>().transform;
        }

        private void Update()
        {
            Material.SetVector("_Center", playerTransform.position);
        }
    }
}