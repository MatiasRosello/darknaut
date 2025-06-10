using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenGlassCollider : MonoBehaviour
{
    private Transform player;
    public float intensityMod = 1.5f;

    private void Start()
    {
        player = GetComponent<Transform>();
    }
    public void AmplifySound(Vector3 position, float intensity)
    {
        Debug.Log($"[SoundManager] CAMINANDO EN VIDRIO en {position}, intensidad {intensity * intensityMod}");
        SoundManager.Instance.MakeNoise(player.position, intensity * intensityMod);
    }
}
