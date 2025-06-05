using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    [SerializeField] private float noiseIntensity = 1f;

    [SerializeField] private float destroyDelay = 0.2f;

    private bool hasMadeNoise = false;

    private void OnCollisionEnter(Collision collision)
    {
        
        if (hasMadeNoise) return;   //evita que haga sonido si colisiones multiples veces

        hasMadeNoise = true;

        SoundManager.Instance.MakeNoise(transform.position, noiseIntensity);

        Destroy(gameObject, destroyDelay);
    }

}
