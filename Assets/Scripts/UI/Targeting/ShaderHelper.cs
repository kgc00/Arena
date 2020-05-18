using Units;
using UnityEngine;
using Utils;
using System.Linq;
using Enums;

namespace UI.Targeting
{
    public class ShaderHelper : MonoBehaviour
    {
        #region Vars
        [HideInInspector] public Transform playerTransform;
        private static readonly int Radius = Shader.PropertyToID("_Radius");
        private static readonly int Border = Shader.PropertyToID("_Border");
        private static readonly int IndicatorType = Shader.PropertyToID("_IndicatorType");
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        private static readonly int AreaColor = Shader.PropertyToID("_AreaColor");
        private static readonly int Center = Shader.PropertyToID("_Center");
        private static readonly int StartPos = Shader.PropertyToID("_StartPos");
        private static readonly int EndPos = Shader.PropertyToID("_EndPos");

        public static int IndicatorTypeVal = 0;

        public static bool isCenterPosPlayerPos = true;
        #endregion
        
        private void Start()
        {
            playerTransform = FindObjectsOfType<Unit>()
                .FirstOrDefault(u => u.Owner.ControlType == ControlType.Local)
                ?.transform;
            Shader.SetGlobalFloat(Radius, 10f);
            Shader.SetGlobalFloat(Border, 1f);
            Shader.SetGlobalFloat(IndicatorType, 0);

            Shader.SetGlobalColor(BaseColor, new Color(0.772549f, 0.772549f, 0.772549f));
            Shader.SetGlobalColor(AreaColor, new Color(0.3176471f, 0.6352941f, 0.7372549f));
        }

        private void OnDisable()
        {
            Shader.SetGlobalFloat(Radius, 0);
            Shader.SetGlobalFloat(Border, 0);
            Shader.SetGlobalVector(Center, Vector3.zero);


            Shader.SetGlobalVector(StartPos, Vector3.zero);
            Shader.SetGlobalVector(EndPos, Vector3.zero);
        }

        private void Update()
        {
            if (playerTransform == null) return;
            
            // circle targeting
            Shader.SetGlobalVector(Center, isCenterPosPlayerPos ? playerTransform.position : MouseHelper.GetWorldPosition());
            // line targeting
            Shader.SetGlobalVector(StartPos, playerTransform.position);
            Shader.SetGlobalVector(EndPos,MouseHelper.GetWorldPosition());
        }
    }
}