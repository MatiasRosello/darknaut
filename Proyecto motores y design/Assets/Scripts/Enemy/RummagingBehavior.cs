using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class RummagingBehavior : MonoBehaviour
{
    [Header("Clips de rebuscar")]
    [Tooltip("Lista de clips para reproducir aleatoriamente.")]
    [SerializeField] private AudioClip[] rummageClips;

    [Header("Intervalo entre sonidos (seg)")]
    [Tooltip("Tiempo mínimo entre sonidos.")]
    [SerializeField] private float minInterval = 2f;
    [Tooltip("Tiempo máximo entre sonidos.")]
    [SerializeField] private float maxInterval = 5f;

    private AudioSource mAudioSource;
    private Coroutine rummageRoutine;

    private void Awake()
    {
        mAudioSource = GetComponent<AudioSource>();
    }

    public void StartRummaging()   // Inicia la rutina de rebuscar.
    {
        if (rummageRoutine == null)
        {
            rummageRoutine = StartCoroutine(RummageLoop());
        }
    }
    
    public void StopRummaging()    // Detiene la rutina de rebuscar.
    {
        if (rummageRoutine != null)
        {
            StopCoroutine(rummageRoutine);
            rummageRoutine = null;
        }
    }

    private IEnumerator RummageLoop()
    {
        while (true)
        {
            // Esperamos un intervalo aleatorio
            float wait = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(wait);

            //elegimos un clip al azar
            if (rummageClips != null && rummageClips.Length > 0)
            {
                int i = Random.Range(0, rummageClips.Length);
                mAudioSource.PlayOneShot(rummageClips[i]);
            }
        }
    }
}
