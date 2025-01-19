using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup gameOverScreen;
    [SerializeField]
    private CanvasGroup gameWinScreen;

    private void OnEnable() { PlayerDeathComponent.playerDied += InitiateGameOver; LevelLoader.OnLevelRetry += RetryCleanup; }
    private void OnDisable() { PlayerDeathComponent.playerDied -= InitiateGameOver; LevelLoader.OnLevelRetry -= RetryCleanup; }

    private void Start()
    {
        CleanUpGameOver();
    }

    private void RetryCleanup()
    {
        StartCoroutine(InitiateCleanup());
    }

    private IEnumerator InitiateCleanup()
    {
        yield return new WaitForSecondsRealtime(2);
        CleanUpGameOver();
    }

    private void CleanUpGameOver()
    {
        Time.timeScale = 1.0f;
        gameOverScreen.alpha = 0.0f;
        gameOverScreen.blocksRaycasts = false;
        gameOverScreen.interactable = false;
    }

    public void InitiateGameOver()
    {
        Time.timeScale = 0.0f;
        gameOverScreen.alpha = 1.0f;
        gameOverScreen.blocksRaycasts = true;
        gameOverScreen.interactable = true;
    }

    public void InitiateGameWin()
    {

        Time.timeScale = 0.0f;
        gameWinScreen.alpha = 1.0f;
        gameWinScreen.blocksRaycasts = true;
        gameWinScreen.interactable = true;
    }
}
