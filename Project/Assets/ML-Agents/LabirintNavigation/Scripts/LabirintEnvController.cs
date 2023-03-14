using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Event
{
    HitWall = 1,
    StepForward = 2,  
    StepNotForward = 3, 
    StepedOnNewFloor = 4,
    Rotate = 5,
    
}

public class LabirintEnvController : MonoBehaviour
{
    private GameObject[] AllObjWithTag;

    public LabirintAgent labirintAgent;

    private Vector3 startPosition;

    List<Renderer> RenderersList = new List<Renderer>();

    Rigidbody labirintAgentRb;

    [SerializeField]
    public MazeSpawner mazeSpawner;

    private int resetTimer;
    public int MaxEnvironmentSteps;

    void Start()
    {
                // Used to control agent & ball starting positions
       

        labirintAgentRb = labirintAgent.GetComponent<Rigidbody>();
        startPosition = transform.position;
       // startPosition.y -= transform.position.y;
        //mazeSpawner = GetComponent<MazeSpawner>();
       
        ResetScene();
    } 


    public void ResolveEvent(Event triggerEvent)
    {
        switch (triggerEvent)
        {
            case Event.HitWall:
               
                 // apply penalty
                labirintAgent.AddReward(-200f);
                labirintAgent.EndEpisode();

                ResetScene();
                break;            

            case Event.StepForward:                
                labirintAgent.AddReward(5f);                
                break;
            
            case Event.StepNotForward:                
                labirintAgent.AddReward(-200f);
                labirintAgent.EndEpisode();
                ResetScene();                
                break;
                
            case Event.Rotate:
                labirintAgent.AddReward(5f);                
                break;
                
            case Event.StepedOnNewFloor:
                
                labirintAgent.AddReward(20f);
                
                break;
        }
    }



    void FixedUpdate()
    {
        resetTimer += 1;
        if (resetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            labirintAgent.EpisodeInterrupted();

            ResetScene();
        }
    }


    public void ResetScene()
    {
        resetTimer = 0;    
        // randomise starting positions and rotations
        var randomPosX = 5f;// Random.Range(5f, 10f);
        var randomPosZ = 5f;//Random.Range(5f, 10);
        //labirintAgent.transform.position =  startPosition;
        //labirintAgent.transform.eulerAngles = Vector3.zero;
        
        var randomPosY = 1f;
        var randomRot = 0f;//Random.Range(-10f, 10f);

        //labirintAgent.transform.localPosition = new Vector3(randomPosX, startPosition, randomPosZ);
        labirintAgent.transform.localPosition = new Vector3(randomPosX, randomPosY - startPosition.y, randomPosZ);
        labirintAgent.transform.eulerAngles = new Vector3(0, randomRot, 0);

        mazeSpawner.Restart();
    }
   
}