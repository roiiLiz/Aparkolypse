using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup gameOverScreen;

    private void OnEnable() { PlayerDeathComponent.playerDied += InitiateGameOver; }
    private void OnDisable() { PlayerDeathComponent.playerDied -= InitiateGameOver; }

    private void InitiateGameOver()
    {
        Debug.Log("GAME OVER!");
    }
}
