using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cell: IComparable<Cell>
{
    public bool isTree;
    public bool isFire;

    public bool isWalkable;

    public CellTypes cellType;

    public Vector2Int cellPosition;
    public int gCost = 0;
    public int hCost = 0;
    public Cell parentCell;

    public Cell(Vector2Int cellPosition)
    {
        this.cellPosition = cellPosition;
        parentCell = null;
    }

    public Cell(bool isTree)
    {
        this.isTree = isTree;
    }

    public Cell(CellTypes cellType, Vector2 cellPosition)
    {
        this.cellType = cellType;
        //this.cellPosition = cellPosition;
    }

    public void AssignCellType(CellTypes cellType)
    {
        this.cellType = cellType;

        if(cellType == CellTypes.Terrain)
        {
            isWalkable = true;
        }
        else
        {
            isWalkable = false;
        }
    }

    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public void ResetPathfindingData()
    {
        gCost = 0; // or some default value
        hCost = 0;
        parentCell = null;
    }
    public int CompareTo(Cell nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);

        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return compare;
    }
}

