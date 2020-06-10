using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DebugSize : MonoBehaviour
{
    public Material mat;
    void Update()
    {
        //test
        Shader.SetGlobalVector("_DebugCenter", 
        new Vector4(this.transform.position.x, 
            0, this.transform.position.z, 0f));
        Debug.Log(mat.GetVector("_Test"));
        // mat.SetVector("_DebugCenter", 
        //     new Vector4(this.transform.position.x, 
        //         0, this.transform.position.z, 0f));
    }
}
