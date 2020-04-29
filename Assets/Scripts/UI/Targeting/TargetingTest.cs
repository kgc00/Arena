using System;
using Units;
using UnityEngine;
using Utils;

namespace UI.Targeting
{
    [ExecuteInEditMode]
    public class TargetingTest : MonoBehaviour
    {
        public Transform playerTransform;

        private void OnEnable()
        {
            playerTransform = FindObjectOfType<Unit>().transform;
            Shader.SetGlobalFloat("_Radius",10f);
            Shader.SetGlobalFloat("_Border",1f);
            Shader.SetGlobalFloat("_IndicatorType", 1);
            
            Shader.SetGlobalColor("_BaseColor", new Color(0.772549f, 0.772549f, 0.772549f));
            Shader.SetGlobalColor("_AreaColor", new Color(0.3176471f, 0.6352941f, 0.7372549f));
        }

        private void OnDisable()
        {
            Shader.SetGlobalFloat("_Radius",0);
            Shader.SetGlobalFloat("_Border",0);
            Shader.SetGlobalVector("_Center",Vector3.zero);
            
            
            Shader.SetGlobalVector("_StartPos", Vector3.zero);
            Shader.SetGlobalVector("_EndPos",Vector3.zero);
        }
        
        private void Update()
        {
            
            Shader.SetGlobalVector("_StartPos",Vector3.zero);
            Shader.SetGlobalVector("_EndPos",transform.position);
            Shader.SetGlobalVector("_Center",transform.position);
            
            // Shader.SetGlobalVector("_Center", playerTransform.position);
            // Shader.SetGlobalVector("_StartPos", playerTransform.position);
            // Shader.SetGlobalVector("_EndPos",MouseHelper.GetWorldPosition());
        }
    }
}