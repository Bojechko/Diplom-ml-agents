using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class LabirintAgent : Agent
{
    private PushBlockSettings m_PushBlockSettings;

    public Rigidbody m_AgentRb;

    public Rigidbody goal;

    [HideInInspector]
    public LabirintEnvController envController;

    private Vector3 startPosition;

    private float distance = 0;
    //private float newDistance = 0;

    public override void Initialize()
    {
        
        
        m_AgentRb = GetComponent<Rigidbody>();
        distance = Vector3.Distance(startPosition, m_AgentRb.transform.localPosition);
        startPosition = m_AgentRb.transform.position;
        envController = GetComponentInParent<LabirintEnvController>();
     
    }

    public override void CollectObservations(VectorSensor sensor)
    {
      //  sensor.AddObservation(IHaveAKey);
      sensor.AddObservation(this.transform.rotation.y);
      sensor.AddObservation(m_AgentRb.velocity);
      sensor.AddObservation(this.transform.position);
    }

   
    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];

        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                envController.ResolveEvent(Event.StepForward); 
                break;
         //   case 2:
              //  dirToGo = transform.forward * -1f;
             //   break;
            case 3:
                rotateDir = transform.up * 1f;
                envController.ResolveEvent(Event.Rotate); 
                break;
            case 4:
                rotateDir = transform.up * -1f;
                envController.ResolveEvent(Event.Rotate); 
                break;            
        }
        transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        m_AgentRb.AddForce(dirToGo * 1f,
            ForceMode.VelocityChange);          
       
    }

    void FixedUpdate(){
       /* newDistance = Vector3.Distance(startPosition, m_AgentRb.transform.localPosition);
        if (distance < newDistance){
            envController.ResolveEvent(Event.StepForward);
        } else {*/
            
       /* }
        distance = newDistance;*/
        if (m_AgentRb.velocity.magnitude < 1){
            envController.ResolveEvent(Event.StepNotForward); 
        }       
        
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
       
        MoveAgent(actionBuffers.DiscreteActions);
    }

    void OnCollisionEnter(Collision other)
    {
       if (other.gameObject.CompareTag("wall")){
            envController.ResolveEvent(Event.HitWall);
       }      
        
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
    }
}
