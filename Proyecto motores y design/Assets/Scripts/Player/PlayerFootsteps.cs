using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[System.Serializable]
public class SurfaceData
{
    [Tooltip("Tag que identifica esta superficie (ej. \"Wood\", \"Metal\").")]
    public string surfaceTag;

    [Header("Clips de pisada")]
    public AudioClip walkClip;
    public AudioClip runClip;
    public AudioClip crouchClip;

    [Header("Intesidades de ruido")]
    public float walkIntensity = 1f;
    public float runIntensity = 1.5f;
    public float crouchIntensity = 0.3f;
}


    [RequireComponent(typeof(PlayerMovement))]
public class PlayerFootsteps : MonoBehaviour
{
    [Header("Datos por tipo de superficie")]
    [SerializeField] private SurfaceData[] surfaces;

    [Header("Referencias")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private AudioSource audioSource;

    [Header("Volumen")]
    [Tooltip("Volumen m�ximo de las pisadas (0�1). Se multiplica por la intensidad.")]
    [SerializeField] private float baseFootsetpVolume = 0.7f;

    [Header("Intervalos (segs)")]
    [SerializeField] private float runInterval = 0.4f;  //espera entre ruidos al moverse de diferentes formas
    [SerializeField] private float walkInterval = 0.6f;
    [SerializeField] private float crouchInterval = 1.2f;

    [Header("Intensidad ruido")]
    [SerializeField] private float defaultWalkIntensity = 1f;
    [SerializeField] private float defaultRunIntensity = 1.5f;
    [SerializeField] private float defaultCrouchIntensity = 0.3f;

    [Header("Audio clips")]
    [SerializeField] private AudioClip defaultWalkClip;
    [SerializeField] private AudioClip defaultRunClip;
    [SerializeField] private AudioClip defaultCrouchClip;

    private float nextStepTime = 0f;

    private void Awake()
    {
        if(playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }



    // Update is called once per frame
    void Update()
    {
        //Solo emitimos ruido si est� en el suelo y hay input de movimiento
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (!playerMovement.IsGrounded || moveInput.magnitude == 0f) return;

        //Determinamos si esta corriendo o si esta agachado
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isCrouched = Input.GetKey(KeyCode.LeftControl);

        // 2. Determinamos tipo de superficie
        SurfaceData data = null;
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 5f))
        {
            // Buscamos en el array un SurfaceData que coincida con el tag
            foreach (var s in surfaces)
            {
                if (hit.collider.CompareTag(s.surfaceTag))
                {
                    data = s;
                    break;
                }
            }
        }

        AudioClip walkC   = data != null ? data.walkClip : defaultWalkClip;
        AudioClip runC    = data != null ? data.runClip : defaultRunClip;
        AudioClip crouchC = data != null ? data.crouchClip : defaultCrouchClip;

        float walkInt   = data != null ? data.walkIntensity : defaultWalkIntensity;
        float runInt    = data != null ? data.runIntensity : defaultRunIntensity;
        float crouchInt = data != null ? data.crouchIntensity : defaultCrouchIntensity;

        float interval, intensity;
        AudioClip clipToPlay;

        if (isRunning)
        {
            interval = runInterval;
            intensity = runInt;
            clipToPlay = runC;
        }
        else if (isCrouched)
        {
            interval = crouchInterval;
            intensity = crouchInt;
            clipToPlay = crouchC;
        }
        else
        {
            interval = walkInterval;
            intensity = walkInt;
            clipToPlay = walkC;
        }

        if (Time.time >= nextStepTime)
        {
            //Generamos el evento de ruido
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.MakeNoise(transform.position, intensity);
            }

            //Reproducimos el clip de audio
            if (audioSource != null && clipToPlay != null)
            {
                float volume = Mathf.Clamp01(baseFootsetpVolume * intensity);
                audioSource.PlayOneShot(clipToPlay, volume);
            }

            //Agendamos el siguiente "paso"
            nextStepTime = Time.time + interval;
        }
    }
}
