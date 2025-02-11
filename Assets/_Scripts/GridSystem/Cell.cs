using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cell
{
    public bool isTree;
    public bool isFire;

    public bool isWalkable;

    public CellTypes cellType;

    public Cell()
    {

    }

    public Cell(bool isTree)
    {
        this.isTree = isTree;
    }

    public Cell(CellTypes cellType)
    {
        this.cellType = cellType;
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
}

public enum CellTypes
{
    Tree,
    Fire,
    Terrain
}
