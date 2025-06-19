using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    public static AmbienceManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Evita duplicados si ya hay uno
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Lo mantiene entre escenas
    }
}
