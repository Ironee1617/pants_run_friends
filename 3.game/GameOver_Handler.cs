using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver_Handler : MonoBehaviour
{
    public delegate void GameOver();
    public static event GameOver OnGameOver;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            OnGameOver();
    }
}
