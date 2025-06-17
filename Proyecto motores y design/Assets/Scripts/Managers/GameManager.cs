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

    public void Lose()
    {
        Debug.Log("[GameManager] Derrota: cargando escena Game Over.");
        SceneManager.LoadScene(gameOverScene);
    }

    public void Win()
    {
        Debug.Log("[GameManager] Victoria: cargando escena Victory.");
        SceneManager.LoadScene(victoryScene);
    }
}
