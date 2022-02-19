using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Utils;

namespace Tests.Play_Mode {
    public class Sandbox : MonoBehaviour, IMonoBehaviourTest {
        private static float dest = 5f;
        // mouse cursor
        private Vector3 targetLocation = new Vector3(dest, 0, dest),
            // player position
            startLocation = new Vector3(-2, 0, 3),
            // enemy position
            unitLocation = new Vector3(9, 0, 7),
        // enemy position
        unitLocation2 = new Vector3(3, 0, 5);

        private float Range = 20f,
            Force = 250f;

        public bool IsTestFinished { get; private set; }

        [UnityTest]
        public IEnumerator SandboxWithEnumeratorPasses() {
            Vector3 heading = targetLocation - startLocation;
            Debug.Log($"heading {heading}");
            heading.y = 0;        
            var distance = heading.magnitude;            
            Debug.Log($"distance {distance}");
            var direction =  heading / distance;
            Debug.Log($"direction {direction}");
            var center = startLocation - (direction * Range / 2);
            Debug.Log($"center {center}");
            var size = Range; 
            
            
            
            // var pullComponent = new GameObject("Pull Force").AddComponent<BoxCollider>().gameObject
            //                                                         .AddComponent<PullComponent>().Initialize(Force, size, center);

            // Debug.Log(pullComponent.transform.position);
            yield return null;
            // var unit = Instantiate(SpawnHelper.PrefabFromUnitType(Types.Melee), unitLocation, Quaternion.identity);
            
            // Debug.Log(unit.transform.position);
            // yield return new WaitForSeconds(1.5f);
            // Debug.Log(unit.transform.position);
            // Destroy(unit);
            // unit = Instantiate(SpawnHelper.PrefabFromUnitType(Types.Melee), unitLocation2, Quaternion.identity);
            //
            // Debug.Log(unit.transform.position);
            // yield return new WaitForSeconds(1.5f);
            // Debug.Log(unit.transform.position);
            IsTestFinished = true;
        }
    }
}