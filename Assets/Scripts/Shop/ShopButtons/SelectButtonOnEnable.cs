using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectButtonOnEnable : MonoBehaviour
{
    public Button targetButton; // Assign your button in the Inspector

    void OnEnable()
    {
        // Set the target button as the selected GameObject
        if (targetButton != null)
        {
            EventSystem.current.SetSelectedGameObject(targetButton.gameObject);
        }
    }
}
