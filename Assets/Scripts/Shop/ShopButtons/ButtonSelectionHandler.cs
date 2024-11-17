using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionHandler : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private GameObject targetObject;  // The first child of the button object

    void Start()
    {
        // Get the first child of this GameObject (assuming it exists)
        if (transform.childCount > 0)
        {
            targetObject = transform.GetChild(0).gameObject;
        }
    }

    // Called when the button is selected
    public void OnSelect(BaseEventData eventData)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);  // Activate the target object
        }
    }

    // Called when the button is deselected
    public void OnDeselect(BaseEventData eventData)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);  // Deactivate the target object
        }
    }
}

