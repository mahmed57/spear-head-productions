using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject options_menu;
    public GameObject credit_menu;

    public void play_button_action()
    {
        SceneManager.LoadScene(1);
    }

    public void options_button_enter()
    {
        options_menu.SetActive(true);
    }

    public void options_menu_exit()
    {
        options_menu.SetActive(false);
    }

    public void credits_menu_enter()
    {
        credit_menu.SetActive(true);
    }

    public void credits_menu_exit()
    {
        credit_menu.SetActive(false);
    }

    public void quit_button_action()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
