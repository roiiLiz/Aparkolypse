using Unity.VisualScripting;
using UnityEngine;

public class PauseManager : MonoBehaviour 
{
    [SerializeField]
    private GameObject pauseParent;
    [SerializeField]
    private CanvasGroup pauseMenu;

    private bool isPaused = false;
    // private float currentTimeScale = 1f;
    // public bool IsPaused { get { return isPaused; } }

    private void Start()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        ActivatePause(isPaused);
    }

    private void ActivatePause(bool isPaused)
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
        } else
        {
            Time.timeScale = 1f;
        }

        pauseParent.SetActive(isPaused);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}
