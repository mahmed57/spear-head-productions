using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject options_menu;
    public GameObject credit_menu;

    private void Start()
    {
        DestroyDontDestroyOnLoadObjects();

        options_menu.SetActive(false);
        credit_menu.SetActive(false);

        Time.timeScale = 1f;
        
    }

    public void play_button_action()
    {
        SceneManager.LoadScene(1);
    }

    public void options_button_enter()
    {
        options_menu.SetActive(true);

        transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);

    }

    public void options_menu_exit()
    {
        options_menu.SetActive(false);

        transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);

    }

    public void credits_menu_enter()
    {
        credit_menu.SetActive(true);

        transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);

    }

    public void credits_menu_exit()
    {
        credit_menu.SetActive(false);

        transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);

    }

    public void quit_button_action()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void DestroyDontDestroyOnLoadObjects()
    {
        
        GameObject[] rootObjects = FindObjectsOfType<GameObject>();

        
        foreach (GameObject obj in rootObjects)
        {
            
            if (obj.scene.name == null || obj.scene.name == "DontDestroyOnLoad")
            {
                
                Destroy(obj);
            }
        }
    }
}
