using Fractura.CraftingSystem;
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

    [SerializeField] private List<CraftingObject> craftingObjectIngredients;
    [SerializeField] private GameObject lootCell;

    [Header("Set Bounds")]
    [SerializeField] private Transform circleParent;
    [SerializeField] private GameObject wizardPrefab;
    [SerializeField] private GameObject centerPrefab;
    [SerializeField] private int minXCircle;
    [SerializeField] private int maxXCircle;
    [SerializeField] private int minYCircle;
    [SerializeField] private int maxYCircle;

    [Header("TEST")]
    public Transform pathParent;
    public GameObject testPrefab;
    public Vector3Int? startGridPosition;
    public Vector3Int? endGridPosition;
    public Stack<Vector3> pathStack = new();

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
        DrawWizardsAndClearInterior();


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

    private void DrawWizardsAndClearInterior()
    {
        // Calculate the center of the circle based on your bounds.
        float centerX = (minXCircle + maxXCircle) / 2f;
        float centerY = (minYCircle + maxYCircle) / 2f;
        Vector2 center = new Vector2(centerX, centerY);

        // Determine the radius as half the smaller dimension of the bounds.
        float radius = Mathf.Min(maxXCircle - minXCircle, maxYCircle - minYCircle) / 2f;
        float radiusSquared = radius * radius;

        // Define tolerances:
        float edgeTolerance = 2f;      // How close a cell must be (in squared distance) to be considered on the edge.
        float interiorThreshold = (radius - edgeTolerance) * (radius - edgeTolerance); // Anything strictly inside the edge.

        // First, clear any existing cell objects within the entire bounds.
        // (This assumes your grid objects are all children of this transform.)
        List<Transform> childrenToRemove = new List<Transform>();
        for (int x = minXCircle; x <= maxXCircle; x++)
        {
            for (int y = minYCircle; y <= maxYCircle; y++)
            {
                // We'll later instantiate edge markers if needed.
                // For now, check if there's an existing cell object at (x,y)
                // (This assumes your cell objects are positioned at integer coordinates.)
                // You can do this in various ways. One approach is to iterate over all children:
                foreach (Transform child in transform)
                {
                    Vector2 childPos = child.position;
                    // Check if the child's position corresponds to (x,y)
                    if (Mathf.Approximately(childPos.x, x) && Mathf.Approximately(childPos.y, y))
                    {
                        // Compute distance from center
                        float dx = x - centerX;
                        float dy = y - centerY;
                        float distSquared = dx * dx + dy * dy;

                        // If the cell is strictly inside the circle (not on the edge)
                        if (distSquared < interiorThreshold)
                        {
                            childrenToRemove.Add(child);
                        }
                    }
                }
            }
        }
        // Remove all marked children.
        foreach (Transform t in childrenToRemove)
        {
            Destroy(t.gameObject);
        }

        // Now, loop through the bounds and instantiate wizard edge markers where needed.
        for (int x = minXCircle; x <= maxXCircle; x++)
        {
            for (int y = minYCircle; y <= maxYCircle; y++)
            {
                float dx = x - centerX;
                float dy = y - centerY;
                float distSquared = dx * dx + dy * dy;

                // Check if the cell is near the circle's edge.
                if (Mathf.Abs(distSquared - radiusSquared) <= edgeTolerance)
                {
                    // Instantiate the wizard (edge marker) prefab.
                    GameObject newWizard = Instantiate(wizardPrefab, new Vector2(x, y), Quaternion.identity);
                    newWizard.transform.parent = circleParent;

                    // Mark this cell as an obstacle in A* (set movement penalty to 0).
                    aStarMovementPenalty[x, y] = 0;
                }
                // Optionally, you can handle cells outside the circle differently,
                // but here we only care about the edge and interior.
            }
        }

        // Finally, instantiate the center object in the middle of the circle.
        // If you have a separate prefab for the center, use that (here we assume centerPrefab exists).
        // Otherwise, you can use wizardPrefab.
        if (centerPrefab != null)
        {
            GameObject centerObj = Instantiate(centerPrefab, center, Quaternion.identity);
            centerObj.transform.parent = circleParent;
        }
        else
        {
            // Fallback: reuse wizardPrefab for the center.
            GameObject centerObj = Instantiate(wizardPrefab, center, Quaternion.identity);
            centerObj.transform.parent = circleParent;
        }
    }

    //private void DrawWizards()
    //{
    //    for(int x = minXCircle; x > maxXCircle; x++)
    //    {
    //        var newG = Instantiate(wizardPrefab, new Vector2(x, maxYCircle), Quaternion.identity);
    //        newG.transform.parent = circleParent;

    //        aStarMovementPenalty[x, maxYCircle] = 0;
    //        //grid[x, maxYCircle] = null;
    //    }
    //}

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
