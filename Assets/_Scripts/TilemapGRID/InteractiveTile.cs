using Fractura.CraftingSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class InteractiveTile : MonoBehaviour
{
    [SerializeField] private bool canInteract;
    [SerializeField] private CraftingObject obj;
    
    [Header("UI Settings")]
    // Reference to the TextMeshPro component that will be shown above the interactable object.
    [SerializeField] private TextMeshProUGUI textMeshPro;
    // Offset to position the text above the interactable object.
    [SerializeField] private Vector3 offset = new Vector3(0, 1f, 0);


    private void OnTriggerEnter2D(Collider2D collision)
    {
        canInteract = true;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + offset);
        textMeshPro.transform.position = screenPos;
        textMeshPro.text = $"{obj.ObjectName} (E)";
        textMeshPro.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canInteract = false;

        if (textMeshPro != null)
        {
            textMeshPro.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(canInteract)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Hello");
                Inventory.Instance.AddItem(obj);
                Destroy(gameObject);
            }    
        }
    }

    // This method is called when another collider enters this trigger collider
    //void OnTriggerEnter2D(Collider2D other)
    //{
    //if(Input.GetKeyDown(KeyCode.E))
    //{
    //    Debug.Log("HELLO");
    //}
    //Debug.Log("HELLO");
    // Use the other collider's position as the point of contact.
    // Depending on your setup, you might refine this (e.g., using bounds center or closest point).
    //Vector3 worldPoint = other.transform.position;
    //Vector3 worldPoint = other.ClosestPoint(tilemap.transform.position);

    //// Convert the world point to the corresponding cell position in the tilemap grid.
    //Vector3Int cellPosition = tilemap.WorldToCell(worldPoint);

    //// Retrieve the tile at that cell position.
    //TileBase triggeredTile = tilemap.GetTile(cellPosition);

    //if (triggeredTile != null)
    //{
    //    Debug.Log("Triggered tile: " + triggeredTile.name + " at cell: " + cellPosition);
    //}
    //else
    //{
    //    Debug.Log("No tile found at cell: " + cellPosition);
    //}

    //Vector3 worldPoint = other.bounds.center;
    //Vector3Int cellPosition = tilemap.WorldToCell(worldPoint);
    //TileBase tile = tilemap.GetTile(cellPosition);

    //// If no tile is found at the calculated cell, check nearby cells
    //if (tile == null)
    //{
    //    // Define small offsets to check neighboring cells
    //    Vector3Int[] offsets = {
    //        new Vector3Int(0, 0, 0),
    //        new Vector3Int(1, 0, 0),
    //        new Vector3Int(-1, 0, 0),
    //        new Vector3Int(0, 1, 0),
    //        new Vector3Int(0, -1, 0)
    //    };

    //    foreach (var offset in offsets)
    //    {
    //        TileBase adjacentTile = tilemap.GetTile(cellPosition + offset);
    //        if (adjacentTile != null)
    //        {
    //            tile = adjacentTile;
    //            cellPosition += offset;
    //            break;
    //        }
    //    }
    //}

    //if (tile != null)
    //{
    //    Debug.Log("Tile found: " + tile.name + " at cell: " + cellPosition);
    //}
    //else
    //{
    //    Debug.Log("No tile found near cell: " + cellPosition);
    //}
    //}
}
