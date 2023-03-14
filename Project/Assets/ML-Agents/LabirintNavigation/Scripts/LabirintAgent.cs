using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class LabirintAgent : Agent
{
    private PushBlockSettings m_PushBlockSettings;

    public Rigidbody m_AgentRb;

    //public Rigidbody goal;

    [HideInInspector]
    public LabirintEnvController envController;

    private Vector3 startPosition;

    private float counter = 0;
  //  private float newDistance = 0;
    public GameObject objInFront;

    public override void Initialize()
    {     
        m_AgentRb = GetComponent<Rigidbody>();
      //  distance = Vector3.Distance(startPosition, m_AgentRb.transform.localPosition);
       // startPosition = m_AgentRb.transform.position;
        
        envController = GetComponentInParent<LabirintEnvController>();
     
    }

    public void Start(){
        objInFront = GameObject.FindWithTag("wall");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.rotation.y);//1
        sensor.AddObservation(m_AgentRb.velocity);//3
        sensor.AddObservation(this.transform.position);//3

        Vector3 toObjInFront = new Vector3((objInFront.transform.position.x - this.transform.position.x),
                                        (objInFront.transform.position.y - this.transform.position.y),
                                        (objInFront.transform.position.z - this.transform.position.z));

        sensor.AddObservation(toObjInFront.magnitude); //1
        sensor.AddObservation(toObjInFront.normalized);  //3                     
    }

   
    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        //var action = act[0];

        var dirToGoForwardAction = act[0];
        var rotateDirAction = act[1];
       // var dirToGoSideAction = act[2];
        

        if (dirToGoForwardAction == 1)
            dirToGo =  transform.forward * 1f;
        else if (dirToGoForwardAction == 2)
            dirToGo =  transform.forward * -1f;

        if (rotateDirAction == 1)
            rotateDir = transform.up * -1f;
        else if (rotateDirAction == 2)
            rotateDir = transform.up * 1f;

       /* if (dirToGoSideAction == 1)
            dirToGo = transform.right * -1f;
        else if (dirToGoSideAction == 2)
            dirToGo = transform.right;*/

        

        /*switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;                
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
        }*/
        transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        m_AgentRb.AddForce(dirToGo * 1f, ForceMode.VelocityChange);          
       
    }

    void FixedUpdate(){
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, fwd, out hit)){            
            objInFront =  hit.transform.gameObject;          
                
        }

        /*newDistance = Vector3.Distance(startPosition, m_AgentRb.transform.localPosition);
        if ( -3f < distance - newDistance || distance - newDistance < 3f){
            envController.ResolveEvent(Event.StepForward);
        } 
        distance = newDistance;*/
        /*
        if (m_AgentRb.velocity. < 1){
            envController.ResolveEvent(Event.StepNotForward); 
        }       */

        if (m_AgentRb.velocity.magnitude < 1.0 ){
            counter++;
            if (counter > 20){
                envController.ResolveEvent(Event.StepNotForward); 
                counter = 0;
            }
            
        } else {
            envController.ResolveEvent(Event.StepForward); 
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

       if(other.gameObject.CompareTag("floor")){
            if (!other.gameObject.GetComponent<IFloor>().isStepped){
                    other.gameObject.GetComponent<IFloor>().isStepped = true;
                    envController.ResolveEvent(Event.StepedOnNewFloor);
                }        
               
       }    
        
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
                var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.D))
        {
            // rotate right
            discreteActionsOut[1] = 2;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            // forward
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            // rotate left
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            // backward
            discreteActionsOut[0] = 2;
        }
      /*  if (Input.GetKey(KeyCode.LeftArrow))
        {
            // move left
            discreteActionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // move right
            discreteActionsOut[2] = 2;
        }*/
    }
}
