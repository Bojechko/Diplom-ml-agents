using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class IFloor : MonoBehaviour
{
    [HideInInspector]
    public LabirintEnvController envController;
    
    public bool isStepped = false;

    public void Start()
    {
        envController = GetComponent<LabirintEnvController>();
    }

    /* void OnCollisionEnter(Collision other)
    {
       if ((other.gameObject.CompareTag("agent")) && (!isStepped)){
            isStepped = true;
            envController.ResolveEvent(Event.StepedOnNewFloor);
       }        
       
    }*/
}
