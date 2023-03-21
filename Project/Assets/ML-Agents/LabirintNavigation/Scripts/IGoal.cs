using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IGoal : MonoBehaviour
{[HideInInspector]
    public LabirintEnvController envController;
    
    public bool isTouched = false;

    //public GameObject obj;

    public void Start()
    {
        envController = GetComponent<LabirintEnvController>();        
    }
}
