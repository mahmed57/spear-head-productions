using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Build.Content;

public class GameOverScreen : MonoBehaviour
{
    private PlayerMovements playerMovements;
    private PlayerHealthManager playerHealth;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerMovements = player.GetComponent<PlayerMovements>();
            playerHealth = player.GetComponent<PlayerHealthManager>();
        }


        if (playerMovements == null || playerHealth == null)
        {
            Debug.LogError("PlayerMovements or MainPlayerHealthManager scripts not found!");
        }

    }
    public void RestartButton()
    {
        SceneManager.LoadScene("Level-1", LoadSceneMode.Single);

        Time.timeScale = 1f;
    }

    public void ExitButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }


}