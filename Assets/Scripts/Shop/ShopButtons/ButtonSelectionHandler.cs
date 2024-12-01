using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionHandler : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private GameObject targetObject;  

    void Start()
    {    

        if (transform.childCount > 0)
        {
            targetObject = transform.GetChild(0).gameObject;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true); 
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }
}
