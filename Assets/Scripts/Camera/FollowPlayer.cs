using UnityEngine;
using  System.Linq;
using Enums;
using Units;

public class FollowPlayer : MonoBehaviour
{
    private Transform unitTransform;
    private Unit unit;
    readonly Vector3 offset = new Vector3(0,10,-10);
    
    void Start()
    {
        Debug.Log("1");
        AssignUnitTransform();
    }

    private void AssignUnitTransform()
    {
        Debug.Log("2");

        var test = FindObjectsOfType<Unit>();
        var test2 = FindObjectsOfType<Unit>().FirstOrDefault(element => element.Owner.ControlType == ControlType.Local);
        
        Debug.Log($"test: {test}");
        // Debug.Log($"test2: {test2}");
        Debug.Log("3");

        unitTransform = FindObjectsOfType<Unit>()
            .FirstOrDefault(element => element.Owner.ControlType == ControlType.Local)
            ?.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (unitTransform == null)
        {
            Debug.Log("null");

            AssignUnitTransform();
            return;
        }
        
        
        var target = new Vector3(
            unitTransform.position.x + offset.x,
            offset.y,
            unitTransform.position.z + offset.z
            );

        
        // transform.position = Vector3.Slerp(transform.position, target, 3f);
        transform.position = target;
    }
}
