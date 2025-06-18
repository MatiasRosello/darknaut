using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button playButton;

    void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            if (GameManager.Instance != null)
                GameManager.Instance.StartGame();
            else
                Debug.LogError("GameManager no encontrado.");
        });
    }
}

