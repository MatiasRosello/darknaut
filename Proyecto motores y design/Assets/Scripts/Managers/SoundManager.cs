using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //singleton
    public static SoundManager Instance { get; private set; }

    public event Action<Vector3, float> OnNoiseMade;

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

    public void MakeNoise(Vector3 position, float intensity)
    {
        Debug.Log($"[SoundManager] MakeNoise en {position}, intensidad {intensity}");
        OnNoiseMade?.Invoke(position, intensity);
    }
}
