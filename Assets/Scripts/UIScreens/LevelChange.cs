using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LevelChange : MonoBehaviour
{
    public GameObject controller_movement;
    Gamepad gamepad;
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadFirstScene();
        }

        gamepad = Gamepad.current;

        if(gamepad != null)
        {
            controller_movement.SetActive(true);

            if (gamepad.startButton.wasPressedThisFrame)
            {
                LoadFirstScene();
            }

        }
        else
        {
            controller_movement.SetActive(false);
        }
    }

    void LoadFirstScene()
    {
        SceneManager.LoadScene(2);
    }

}
