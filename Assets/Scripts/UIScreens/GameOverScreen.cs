using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void RestartButton()
    {
        SceneManager.LoadScene("Level-1", LoadSceneMode.Single);
        DestroyDontDestroyOnLoadObjects();
        Time.timeScale = 1f;
    }

    public void ExitButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
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