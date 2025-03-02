using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class AStar
{
    public static Stack<Vector3> BuildPath(Grid grid, Vector3Int startPosition, Vector3Int endPosition)
    {
        const int defaultAStarMovementPenalty = 40;
        List<Cell> openCells = new List<Cell>();
        HashSet<Cell> closedCells = new HashSet<Cell>();

        Cell startCell = grid.GetGridCell(startPosition.x, startPosition.y);
        Cell endCell = grid.GetGridCell(endPosition.x, endPosition.y);

        Cell endPathCell = FindShortestPath(startCell, endCell, grid, openCells, closedCells);

        if (endPathCell != null)
        {
            return CreatePathStack(endPathCell, grid);
        }

        return null;
    }
    private static Cell FindShortestPath(Cell startCell, Cell endCell, Grid grid, List<Cell> openCells, HashSet<Cell> closedCells)
    {
        openCells.Add(startCell);

        while(openCells.Count > 0)
        {
            openCells.Sort();

            Cell currentCell = openCells[0];
            openCells.RemoveAt(0);

            if(currentCell == endCell)
            {
                return currentCell;
            }

            closedCells.Add(currentCell);

            EvaluateNeighbourCells(currentCell, endCell, grid, openCells, closedCells);
        }


        return null;
    }

    private static void EvaluateNeighbourCells(Cell currentCell, Cell endCell, Grid grid, List<Cell> openCells, HashSet<Cell> closedCells)
    {
        Vector2Int currentCellGridPosition = currentCell.cellPosition;

        Cell validNeighbourCell;

        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if(i == 0 && j == 0)
                {
                    continue;
                }

                validNeighbourCell = GetValidNeighbourCell(currentCellGridPosition.x + i, currentCellGridPosition.y + j, grid, closedCells);

                //if (!validNeighbourCell.isWalkable)
                //    validNeighbourCell = null;

                if(validNeighbourCell != null)
                {
                    int newCostToNeighbour;

                    int movementPenaltyForGridSpace = grid.aStarMovementPenalty[validNeighbourCell.cellPosition.x, 
                        validNeighbourCell.cellPosition.y];

                    newCostToNeighbour = currentCell.gCost + GetDistance(currentCell, validNeighbourCell) + movementPenaltyForGridSpace;

                    bool isValidNeighbourNodeInOpenList = openCells.Contains(validNeighbourCell);

                    if (newCostToNeighbour < validNeighbourCell.gCost || !isValidNeighbourNodeInOpenList)
                    {
                        validNeighbourCell.gCost = newCostToNeighbour;
                        validNeighbourCell.hCost = GetDistance(validNeighbourCell, endCell);
                        validNeighbourCell.parentCell = currentCell;

                        if(!isValidNeighbourNodeInOpenList)
                        {
                            openCells.Add(validNeighbourCell);
                        }
                    }
                }
            }
        }
    }

    private static int GetDistance(Cell cellA, Cell cellB)
    {
        int dstX = Mathf.Abs(cellA.cellPosition.x - cellB.cellPosition.x);
        int dstY = Mathf.Abs(cellA.cellPosition.y - cellB.cellPosition.y);

        if(dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }

        return 14 * dstX + 10 * (dstY - dstX);
    }

    private static Cell GetValidNeighbourCell(int cellX, int cellY, Grid grid, HashSet<Cell> closedCells)
    {
        Cell neighbourCell = grid.GetGridCell(cellX, cellY);

        int movementPenaltyForGridSpace = grid.aStarMovementPenalty[cellX, cellY];

        if(movementPenaltyForGridSpace == 0 || closedCells.Contains(neighbourCell))
        {
            return null;
        }
        else
        {
            return neighbourCell;
        }
    }

    private static Stack<Vector3> CreatePathStack(Cell endPathCell, Grid grid)
    {
        Stack<Vector3> movementPathStack = new Stack<Vector3>();

        Cell nextCell = endPathCell;

        //Vector3 nodeMidPoint = grid.cel
        while(nextCell != null)
        {
            Vector3 worldPosition = new Vector3Int(nextCell.cellPosition.x, nextCell.cellPosition.y, 0);

            movementPathStack.Push(worldPosition);

            nextCell = nextCell.parentCell;
        }

        return movementPathStack;
    }
}
