using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject options_menu;
    public GameObject credit_menu;
    public Button playButton;
    public Button optionsButton;
    public Button creditsButton;
    public Button quitButton;
    public Button optionsBackButton;
    public Button creditsBackButton;

    private Button[] mainMenuButtons;
    private int currentIndex = 0;
    private bool canNavigate = true;
    public float inputDelay = 0.2f;

    private enum MenuState { MainMenu, OptionsMenu, CreditsMenu }
    private MenuState currentMenuState = MenuState.MainMenu;

    public Material normalMaterial;
    public Material highlightedMaterial;

    private void Start()
    {
        mainMenuButtons = new Button[] { playButton, creditsButton, optionsButton, quitButton };

        EventSystem.current.SetSelectedGameObject(mainMenuButtons[currentIndex].gameObject);

        foreach (Button btn in mainMenuButtons)
        {
            SetButtonMaterial(btn, normalMaterial);
        }

        options_menu.SetActive(false);
        credit_menu.SetActive(false);

        DestroyDontDestroyOnLoadObjects();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (!canNavigate)
            return;

        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        switch (currentMenuState)
        {
            case MenuState.MainMenu:
                HandleMainMenuInput(verticalInput, horizontalInput);
                break;
            case MenuState.OptionsMenu:
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel"))
                {
                    options_menu_exit();
                }
                break;
            case MenuState.CreditsMenu:

                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel"))
                {
                    credits_menu_exit();
                }
                break;
        }
    }

    private void HandleMainMenuInput(float verticalInput, float horizontalInput)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || verticalInput > 0.5f)
        {
            currentIndex -= 2;
            if (currentIndex < 0)
                currentIndex += mainMenuButtons.Length;

            UpdateSelection();
            StartCoroutine(InputDelay());
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || verticalInput < -0.5f)
        {
            currentIndex += 2;
            if (currentIndex >= mainMenuButtons.Length)
                currentIndex -= mainMenuButtons.Length;

            UpdateSelection();
            StartCoroutine(InputDelay());
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || horizontalInput < -0.5f)
        {
            if (currentIndex % 2 == 0)
            {
                currentIndex += 1;
            }
            else
            {
                currentIndex -= 1;
            }

            UpdateSelection();
            StartCoroutine(InputDelay());
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || horizontalInput > 0.5f)
        {
            if (currentIndex % 2 == 0)
            {
                currentIndex += 1;
            }
            else
            {
                currentIndex -= 1;
            }

            UpdateSelection();
            StartCoroutine(InputDelay());
        }

        // Select the current button with Enter key or gamepad "Submit" button
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit"))
        {
            mainMenuButtons[currentIndex].onClick.Invoke();
        }
    }

    private void UpdateSelection()
    {
        // Update the selected GameObject in the EventSystem
        EventSystem.current.SetSelectedGameObject(mainMenuButtons[currentIndex].gameObject);

        // Update button materials
        for (int i = 0; i < mainMenuButtons.Length; i++)
        {
            if (i == currentIndex)
            {
                SetButtonMaterial(mainMenuButtons[i], highlightedMaterial); // Set to highlighted material
            }
            else
            {
                SetButtonMaterial(mainMenuButtons[i], normalMaterial); // Set to normal material
            }
        }
    }

    private void SetButtonMaterial(Button button, Material material)
    {
        Image img = button.GetComponent<Image>();
        if (img != null)
        {
            img.material = material;
        }
    }

    IEnumerator InputDelay()
    {
        canNavigate = false;
        yield return new WaitForSeconds(inputDelay);
        canNavigate = true;
    }

    public void play_button_action()
    {
        SceneManager.LoadScene(1);
    }

    public void options_button_enter()
    {
        options_menu.SetActive(true);
        currentMenuState = MenuState.OptionsMenu;

        // Disable main menu buttons
        foreach (Button btn in mainMenuButtons)
        {
            btn.interactable = false;
        }

        // Set focus to the back button in options menu
        EventSystem.current.SetSelectedGameObject(optionsBackButton.gameObject);
    }

    public void options_menu_exit()
    {
        options_menu.SetActive(false);
        currentMenuState = MenuState.MainMenu;

        // Enable main menu buttons
        foreach (Button btn in mainMenuButtons)
        {
            btn.interactable = true;
        }

        // Return focus to previously selected main menu button
        UpdateSelection();
    }

    public void credits_menu_enter()
    {
        credit_menu.SetActive(true);
        currentMenuState = MenuState.CreditsMenu;

        // Disable main menu buttons
        foreach (Button btn in mainMenuButtons)
        {
            btn.interactable = false;
        }

        // Set focus to the back button in credits menu
        EventSystem.current.SetSelectedGameObject(creditsBackButton.gameObject);
    }

    public void credits_menu_exit()
    {
        credit_menu.SetActive(false);
        currentMenuState = MenuState.MainMenu;

        // Enable main menu buttons
        foreach (Button btn in mainMenuButtons)
        {
            btn.interactable = true;
        }

        // Return focus to previously selected main menu button
        UpdateSelection();
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
