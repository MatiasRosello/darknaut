using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Opciones");
    }
        
    public void StartGame()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void RetryLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game closed"); // Solo para editor
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void LoadLevelComplete()
    {
        int escenaActualIndex = SceneManager.GetActiveScene().buildIndex;
        int totalEscenas = SceneManager.sceneCountInBuildSettings;

        if (escenaActualIndex + 1 < totalEscenas)
        {
        // Carga la siguiente escena en el orden del Build Settings
            SceneManager.LoadScene(escenaActualIndex + 1);
        }
    else
        {
        // Si no hay más escenas, carga la de victoria
            SceneManager.LoadScene("YouWin");
        }
    }
}
