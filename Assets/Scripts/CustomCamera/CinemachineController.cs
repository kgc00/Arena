using System;
using System.Collections;
using System.Linq;
using Data.Types;
using Units;
using Cinemachine;
using UnityEngine;

namespace CustomCamera {
    public class CinemachineController : MonoBehaviour {
        private Transform _unitTransform;
        private CinemachineVirtualCamera vcam;
        void Start()
        {
            StartCoroutine(AssignUnitTransform());
            vcam = FindObjectOfType<CinemachineVirtualCamera>() ?? throw new Exception("Unable to find vcam");
        }

        private IEnumerator AssignUnitTransform()
        {
            yield return new WaitUntil(() => FindObjectsOfType<CinemachineTargetGroup>()?.FirstOrDefault());

            _unitTransform = FindObjectsOfType<CinemachineTargetGroup>()?.FirstOrDefault()?.transform;
            vcam.Follow = _unitTransform;
            vcam.LookAt = _unitTransform;
        }
    }
}