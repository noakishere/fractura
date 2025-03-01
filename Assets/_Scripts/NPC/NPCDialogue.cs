using Fractura.CraftingSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private bool canInteract;

    [SerializeField] private string dialogue;
    //[SerializeField] private List<string> nextDialogues;
    [Header("UI Settings")]
    //[SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Vector3 offset = new Vector3(0, 1f, 0);



    private void OnTriggerEnter2D(Collider2D collision)
    {
        canInteract = true;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + offset);

        CraftingUIManager.Instance.ShowNPCTextOnScreen(screenPos);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canInteract = false;

        CraftingUIManager.Instance.HideNPCTextOnScreen();
    }

    private void Update()
    {
        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CraftingUIManager.Instance.WriteNPCText(dialogue);
                //textMeshPro.text = dialogue;
            }
        }
    }

    public void ChangeDialogue(string dialogue)
    {
        this.dialogue = dialogue;
    }
}
