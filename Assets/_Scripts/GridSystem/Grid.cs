using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : SingletonMonoBehaviour<Grid>
{
    [SerializeField] private int gridSize;
    [SerializeField] private float noiseScale;
    [SerializeField] private float treeLevel;
    [SerializeField] private float FireLevel;

    [SerializeField] private List<GameObject> treePrefabs;
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private GameObject terrainPrefab;

    [SerializeField] private Cell[,] grid;
    [SerializeField] public int[,] aStarMovementPenalty;

    [Header("TEST")]
    public Vector3Int? startGridPosition;
    public Vector3Int? endGridPosition;
    public Stack<Vector3> pathStack = new();
    public Transform pathParent;
    public GameObject testPrefab;

    //public Vector3Int noValue = new Vector3Int(-9999, -9999, -9999);


    private void Start()
    {
        float[,] noiseMap = new float[gridSize, gridSize];
        float xOffset = Random.Range(-10000f, 10000f);
        float yOffset = Random.Range(-10000f, 10000f);

        float[,] fireNoiseMap = new float[gridSize, gridSize];
        float xFireOffset = Random.Range(-10000f, 10000f);
        float yFireOffset = Random.Range(-10000f, 10000f);

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * noiseScale + xOffset, y * noiseScale + yOffset);
                noiseMap[x, y] = noiseValue;
                
                fireNoiseMap[x, y] = Mathf.PerlinNoise(x * noiseScale + xFireOffset, y * noiseScale + yFireOffset);
            }
        }


        grid = new Cell[gridSize, gridSize];

        for(int x = 0; x < gridSize; x++)
        {
            for(int y = 0; y < gridSize; y++)
            {
                Cell cell = new Cell(new Vector2Int(x, y));
                cell.AssignCellType(CellTypes.Terrain);
                if (noiseMap[x,y] < treeLevel)
                {
                    cell.AssignCellType(CellTypes.Tree);
                }

                if (fireNoiseMap[x, y] > FireLevel)
                {
                    cell.AssignCellType(CellTypes.Fire);
                }
                //cell.isTree = noiseMap[i, j] < treeLevel;
                grid[x, y] = cell;
            }
        }
        CalculateObstacles();
        DrawCell(grid);


        //TEST
        startGridPosition = null;
        endGridPosition = null;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(pathStack != null)
                pathStack = null;
            startGridPosition = null;
            endGridPosition = null;

            foreach(Transform t in pathParent)
            {
                Destroy(t.gameObject);
            }

            for (int i = 0; i < gridSize; i++)
            {
                for(int j = 0; j < gridSize; j++)
                {
                    grid[i, j].ResetPathfindingData();
                }
            }

        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            DisplayPath();
        }
    }

    private void DisplayPath()
    {
        if (startGridPosition != null && endGridPosition != null)
        {
            //pathStack = new();
            pathStack = AStar.BuildPath(this, (Vector3Int)startGridPosition, (Vector3Int)endGridPosition);
        }

        else
            pathStack = null;

        if (pathStack == null)
        {
            Debug.Log("Womp womp");
        }
        else
        {
            foreach(Vector3 worldPosition in pathStack)
            {
                var newG = Instantiate(testPrefab, worldPosition, Quaternion.identity);
                newG.transform.parent = pathParent;
            }
        }

        //foreach(Vector3 worldPosition in pathStack)
        //{
        //    //Gizmos.color = Color.green;
        //}
    }

    public void CalculateObstacles()
    {
        //aStarMovementPenalty = new int[gridSize, gridSize];

        //for(int x = 0; x < gridSize; x++)
        //{
        //    for(int y = 0; y < gridSize; y++)
        //    {
        //        aStarMovementPenalty[x, y] = 40;

        //        foreach(Cell c in grid)
        //        {
        //            if(!c.isWalkable)
        //            {
        //                aStarMovementPenalty[x, y] = 0;
        //                break;
        //            }
        //        }

        //    }
        //}

        aStarMovementPenalty = new int[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                // Only check the cell at (x, y) instead of iterating over all cells
                if (!grid[x, y].isWalkable)
                {
                    aStarMovementPenalty[x, y] = 0; // Mark as an obstacle
                }
                else
                {
                    aStarMovementPenalty[x, y] = 40; // Default movement penalty
                }
            }
        }

        //foreach(int i in aStarMovementPenalty)
        //{
        //    Debug.Log(i);
        //}

    }

    public Cell GetGridCell(int x, int y)
    {
        //Debug.Log($"{x} , {y}");
        if(x < gridSize && y < gridSize && x > -1 && y > -1)
        {
            return grid[x, y];
        }
        else
        {
            //Debug.Log("Requested grid is out of bounds");
            return null;
        }
    }

    private void DrawCell(Cell[,] grid)
    {
        for(int i = 0; i < gridSize; i++)
        {
            for(int j = 0; j < gridSize; j++)
            {
                Cell c = grid[i, j];

                if(c.cellType == CellTypes.Tree)
                {
                    GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Count)];
                    GameObject tree = Instantiate(prefab, transform.position, Quaternion.identity);
                    tree.transform.parent = transform;  
                    tree.transform.position = new Vector2(i, j);

                    tree.GetComponent<CellBehaviour>().cell = c;
                }
                else if(c.cellType == CellTypes.Fire)
                {
                    GameObject fire = Instantiate(firePrefab, transform.position, Quaternion.identity);
                    fire.transform.parent = transform;
                    fire.transform.position = new Vector2(i, j);

                    fire.GetComponent<CellBehaviour>().cell = c;
                }
                else if(c.cellType == CellTypes.Terrain)
                {
                    GameObject terrain = Instantiate(terrainPrefab, transform.position, Quaternion.identity);
                    terrain.transform.parent = transform;
                    terrain.transform.position = new Vector2(i, j);

                    terrain.GetComponent<CellBehaviour>().cell = c;
                }
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (!Application.isPlaying) return;
    //    for (int i = 0; i < gridSize; i++)
    //    {
    //        for (int j = 0; j < gridSize; j++)
    //        {
    //            Cell cell = grid[i, j];

    //            if (cell.isTree)
    //            {
    //                Gizmos.color = Color.green;
    //            }
    //            else
    //            {
    //                Gizmos.color = Color.black;
    //            }
    //            Vector2 pos = new Vector2(i, j);
    //            Gizmos.DrawCube(pos, Vector3.one);
    //        }
    //    }

    //    if (pathStack != null)
    //    {
    //        Gizmos.color = Color.red;
    //        foreach (Vector3 pos in pathStack)
    //        {
    //            Gizmos.DrawCube(pos, Vector3.one);
    //        }
    //    }

    //}
}
