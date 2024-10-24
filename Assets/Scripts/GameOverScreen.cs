using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Build.Content;

public class GameOverScreen : MonoBehaviour
{
    private PlayerMovements playerMovements;
    private MainPlayerHealthManager playerHealth;
    private WallDetection wallDetection;


    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerMovements = player.GetComponent<PlayerMovements>();
            playerHealth = player.GetComponent<MainPlayerHealthManager>();
            wallDetection = player.GetComponent<WallDetection>();
        }


        if (playerMovements == null || playerHealth == null)
        {
            Debug.LogError("PlayerMovements or MainPlayerHealthManager scripts not found!");
        }

    }
    public void RestartButton()
    {
        if (playerHealth != null)
        {
            playerHealth.currentHealth = playerHealth.maxHealth;
            playerHealth.healthBarFill.fillAmount = playerHealth.currentHealth / playerHealth.maxHealth;
        }

        if (playerMovements != null)
        {
            playerMovements.transform.position = new Vector3(0, 0, 0);
        }


        if (wallDetection != null)
        {
            wallDetection.isTouchingWallLeft = false;
            wallDetection.isTouchingWallRight = false;
            wallDetection.isTouchingWallBottom = false;
            wallDetection.isTouchingWallTop = false;
        }


        SceneManager.LoadScene("Level-1", LoadSceneMode.Single);


        Time.timeScale = 1f;
    }

    public void ExitButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }



}