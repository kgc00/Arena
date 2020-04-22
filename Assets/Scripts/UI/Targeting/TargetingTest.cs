using System;
using Units;
using UnityEngine;
using Utils;

namespace UI.Targeting
{
    public class TargetingTest : MonoBehaviour
    {
        public Transform playerTransform;

        private void OnEnable()
        {
            playerTransform = FindObjectOfType<Unit>().transform;
            Shader.SetGlobalFloat("_Radius",10f);
            Shader.SetGlobalFloat("_Border",1f);
            Shader.SetGlobalVector("_Center",Vector3.zero);
            Shader.SetGlobalFloat("_IndicatorType", 1);
        }

        private void OnDisable()
        {
            Shader.SetGlobalFloat("_Radius",0);
            Shader.SetGlobalFloat("_Border",0);
            Shader.SetGlobalVector("_Center",Vector3.zero);
            Shader.SetGlobalFloat("_IndicatorType", 0);
        }

        private void Update()
        {
            Shader.SetGlobalVector("_Center",playerTransform.position);
            Shader.SetGlobalVector("_StartPos", playerTransform.position);
            Shader.SetGlobalVector("_EndPos",MouseHelper.GetWorldPosition());
        }
    }
}