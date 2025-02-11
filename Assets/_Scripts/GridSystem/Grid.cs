using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private int gridSize;
    [SerializeField] private float noiseScale;
    [SerializeField] private float treeLevel;
    [SerializeField] private float FireLevel;

    [SerializeField] private List<GameObject> treePrefabs;
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private GameObject terrainPrefab;

    [SerializeField] private Cell[,] grid;

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

        for(int i = 0; i < gridSize; i++)
        {
            for(int j = 0; j < gridSize; j++)
            {
                Cell cell = new Cell();
                cell.AssignCellType(CellTypes.Terrain);
                if (noiseMap[i,j] < treeLevel)
                {
                    cell.AssignCellType(CellTypes.Tree);
                }

                if (fireNoiseMap[i, j] > FireLevel)
                {
                    cell.AssignCellType(CellTypes.Fire);
                }
                //cell.isTree = noiseMap[i, j] < treeLevel;
                grid[i, j] = cell;
            }
        }

        DrawCell(grid);
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
                }
                else if(c.cellType == CellTypes.Fire)
                {
                    GameObject fire = Instantiate(firePrefab, transform.position, Quaternion.identity);
                    fire.transform.parent = transform;
                    fire.transform.position = new Vector2(i, j);
                }
                else if(c.cellType == CellTypes.Terrain)
                {
                    GameObject terrain = Instantiate(terrainPrefab, transform.position, Quaternion.identity);
                    terrain.transform.parent = transform;
                    terrain.transform.position = new Vector2(i, j);
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

    //            if(cell.isTree)
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

    //}
}
