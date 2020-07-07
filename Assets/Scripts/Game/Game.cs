using System.Collections;
using System.Collections.Generic;
using Players;
using UnityEngine;

public class Game : MonoBehaviour {
    private static Game instance;
    public static Game Instance => instance;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
        
        DontDestroyOnLoad(gameObject);
    }
}