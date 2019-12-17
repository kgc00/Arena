using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPositionToParent : MonoBehaviour
{
    private Transform anchor;
    // Start is called before the first frame update
    void Awake()
    {
        anchor = transform.parent.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = anchor.position;
    }
}
