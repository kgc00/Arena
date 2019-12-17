using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform playerTransform;
    private Unit unit;
    readonly Vector3 offset = new Vector3(0,10,-10);
    
    // Start is called before the first frame update
    void Start()
    {
        // playerTransform = FindObjectOfType<Player>().transform.Find("Unit").transform;
        playerTransform = FindObjectOfType<Unit>().transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerTransform == null)
            return;
        
        
        var target = new Vector3(
            playerTransform.position.x + offset.x,
            offset.y,
            playerTransform.position.z + offset.z
            );

        
        // transform.position = Vector3.Slerp(transform.position, target, 3f);
        transform.position = target;
    }
}
