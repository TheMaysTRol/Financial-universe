using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    
    public bool isPaused = false;

    public void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }
}
