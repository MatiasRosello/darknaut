using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("LoadingScreen"); // Cargar pantalla de carga
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene("Opciones"); // Cargar menú de opciones
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }
}