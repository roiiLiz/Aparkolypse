using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static event Action OnLevelRetry;

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);

        Time.timeScale = 1.0f;
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1.0f;

        OnLevelRetry?.Invoke();
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
}
