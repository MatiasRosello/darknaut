using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Button PlayButton;
    public Button ExitButton;

    void Awake()
    {
        PlayButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(3);
        });

        ExitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}