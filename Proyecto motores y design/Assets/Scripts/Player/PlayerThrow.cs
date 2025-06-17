using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform coinSpawnPoint;
    [SerializeField] private float coinLaunchForce = 10f;
    [SerializeField] private float launchColdown = 0.5f;

    [SerializeField] private AudioSource audioSource;

    [Header("Sonido tirar moneda")]
    [SerializeField] private AudioClip coinToss;

    private float lastLaunchTime = 0f;

    private void Update()
    {
        ThrowCoinInput();
    }

    private void ThrowCoinInput()
    {
        if (Input.GetKeyUp(KeyCode.E) && Time.time >= lastLaunchTime + launchColdown)
        {
            ThrowCoin();
            lastLaunchTime = Time.time;
            audioSource.PlayOneShot(coinToss);
        }
    }

    private void ThrowCoin()
    {
        if (coinPrefab == null || coinSpawnPoint == null)
        {
            Debug.LogWarning("No hay coinPrefab o coinSpawnPoint asignado en PlayerMovement.");
            return;
        }

        GameObject coinInstance = Instantiate(coinPrefab, coinSpawnPoint.position, Quaternion.identity);

        Rigidbody coinRb = coinInstance.GetComponent<Rigidbody>();
        if (coinRb != null)
        {
            Vector3 launchDirection = transform.forward.normalized;
            coinRb.AddForce(launchDirection * coinLaunchForce, ForceMode.Impulse);
        }
    }
}
