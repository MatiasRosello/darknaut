using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    [Header("Configuración de Escenas")]
    [Tooltip("Nombre de la escena de Game Over")]
    [SerializeField] private string gameOverScene = "GameOver";
    [Tooltip("Nombre de la escena de Victoria")]
    [SerializeField] private string victoryScene = "Victory";
    [Tooltip("Nombre de la escena de Menu")]
    [SerializeField] private string mainMenuScene = "MainMenu";
    [Tooltip("Nombre de la escena de Opciones")]
    [SerializeField] private string optionsScene = "Options";

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
        SceneManager.LoadScene("LevelComplete");
    }
}
