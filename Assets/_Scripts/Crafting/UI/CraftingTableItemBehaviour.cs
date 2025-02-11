using Fractura.CraftingSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingTableItemBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image img;
    public Image CraftingTableImage => img;

    [SerializeField] private bool isAssigned;
    public bool IsAssigned => isAssigned;

    [SerializeField] private CraftingObject objectReference;
    public CraftingObject ObjectReference => objectReference;

    public void AssignItem(CraftingObject newObject)
    {
        if(!isAssigned)
        {
            objectReference = newObject;
            img.sprite = newObject.ObjectSprite;
            Debug.Log($"{gameObject.name}: Image renderer's image is set to {newObject.ObjectSprite}");
            isAssigned = true;
            ToggleImageRenderer(true);
        }
        else
        {
            Debug.Log($"{gameObject.name}: Image renderer is already assigned");
        }
    }

    public void ToggleImageRenderer(bool state)
    {
        img.enabled = state;
        Debug.Log($"{gameObject.name}: Image renderer is set to {state}");
    }

    public void EmptyItem()
    {
        CraftingManager.Instance.RemoveItemFromTable(objectReference);

        PlayerInventoryUIManager.Instance.FreeObject(objectReference);

        objectReference = null;
        isAssigned = false;
        img.sprite = null;
        ToggleImageRenderer(false);
        Debug.Log($"{gameObject.name}: Emptied and nulled.");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        img.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        img.color = Color.red;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EmptyItem();
    }
}
