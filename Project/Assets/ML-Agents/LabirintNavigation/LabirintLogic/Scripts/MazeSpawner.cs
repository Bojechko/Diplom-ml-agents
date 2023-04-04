using UnityEngine;
using System.Collections.Generic;

public class MazeSpawner : MonoBehaviour
{
    public Cell CellPrefab;
    public Vector3 CellSize = new Vector3(1,1,0);
    public HintRenderer HintRenderer;

    private List<GameObject> cells = new List<GameObject>();
    private List<GameObject> goals = new List<GameObject>();
    
    public GameObject goal;

    public Maze maze;

    private void Start()
    {
        MazeGenerator generator = new MazeGenerator();
        maze = generator.GenerateMaze();

        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                Cell c = Instantiate(CellPrefab, new Vector3(x * CellSize.x - transform.position.x, y * CellSize.y - transform.position.y, y * CellSize.z), Quaternion.identity);

                c.WallLeft.SetActive(maze.cells[x, y].WallLeft);
                c.WallBottom.SetActive(maze.cells[x, y].WallBottom);
                cells.Add(c.gameObject);

                float randomNumber = Random.Range(0, 1);
        
                if(randomNumber == 0){
                    GameObject newGoal = Instantiate(goal, new Vector3(x * CellSize.x - transform.position.x + 5f, y * CellSize.y - transform.position.y + 0.5f, y * CellSize.z + 5f), transform.rotation);
                    goals.Add(newGoal);
                }
            }
        }       

        //HintRenderer.DrawPath();
    }

    public void  Restart()
    {
        for (int i = 0; i < cells.Count; i++){
            GameObject.Destroy(cells[i]);
        }
        for (int i = 0; i < goals.Count; i++){
            GameObject.Destroy(goals[i]);
        }
        Start();
    }

}