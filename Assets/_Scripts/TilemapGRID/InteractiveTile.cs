using Fractura.CraftingSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BoxCollider2D))]
public class InteractiveTile : MonoBehaviour
{
    [SerializeField] private bool canInteract;
    [SerializeField] private List<CraftingObject> objects;
    
    [Header("UI Settings")]
    // Reference to the TextMeshPro component that will be shown above the interactable object.
    [SerializeField] private TextMeshProUGUI textMeshPro;
    // Offset to position the text above the interactable object.
    [SerializeField] private Vector3 offset = new Vector3(0, 1f, 0);


    private void OnTriggerEnter2D(Collider2D collision)
    {
        canInteract = true;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + offset);

        CraftingUIManager.Instance.ShowItemTextOnScreen(screenPos);

        //textMeshPro.transform.position = screenPos;
        //textMeshPro.text = $"(E)";
        //textMeshPro.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canInteract = false;

        CraftingUIManager.Instance.HideItemTextOnScreen();

        //if (textMeshPro != null)
        //{
        //    textMeshPro.gameObject.SetActive(false);
        //}
    }

    private void Update()
    {
        if(canInteract)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Hello");

                foreach(var obj in objects)
                {
                    Inventory.Instance.AddItem(obj);
                }
                Destroy(gameObject);
            }    
        }
    }
}
