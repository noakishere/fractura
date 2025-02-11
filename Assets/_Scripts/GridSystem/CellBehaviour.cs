using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehaviour : MonoBehaviour
{
    public Cell cell;
    private void OnMouseUpAsButton()
    {
       if(Grid.Instance.startGridPosition == null)
       {
            Grid.Instance.startGridPosition = new Vector3Int(cell.cellPosition.x, cell.cellPosition.y, 0);
       }
       else
        {
            Grid.Instance.endGridPosition = new Vector3Int(cell.cellPosition.x, cell.cellPosition.y, 0);
        }
    }
}
