using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[LevelEndTrigger] Jugador lleg� al final: �Victoria!");
            GameManager.Instance.LoadLevelComplete();
        }
    }
}
