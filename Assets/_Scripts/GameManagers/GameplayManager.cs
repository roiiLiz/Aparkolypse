using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameState {START, BUILD, RIDE, WIN, LOSS}

public class GameplayManager : MonoBehaviour
{
    public GameState state;

    public WaveManager waveManager;

    [Header("UI Vars")]
    public GameObject rideSwitchUI;
    public GameObject rideBtnUI;
    public GameObject shopPanelUI;
    

    public CreditManager creditManager;
    public GameOver gameOver;

    private void Start()
    {
        gameOver = gameOver.GetComponent<GameOver>();

        state = GameState.START;
        StartCoroutine(Round1Begin());
    }

    public IEnumerator BuildBegin()
    {
        //units unable to attack
        //build UI
        shopPanelUI.SetActive(true);
        rideBtnUI.SetActive(true);
        yield return null;
    }

    public IEnumerator Round1Begin()
    {
        //units unable to attack

        //start with an amount of POP
        creditManager = creditManager.GetComponent<CreditManager>();

        creditManager.SetCredits(40);

        shopPanelUI.SetActive(true);

        //if a unit was placed, show ride button
        rideBtnUI.SetActive(true);
        yield return null;
    }
    public void RideBegin()
    {
        state = GameState.RIDE;
        shopPanelUI.SetActive(false);
        rideBtnUI.SetActive(false);
        waveManager.waveActive = true;
        StartCoroutine(waveManager.GetComponent<WaveManager>().WaveSpawn());

        //make grid unselectable

        //check castle HP at the end of round
        //if 0, set state to LOSS

    }

    public void RideEnd()
    {
        waveManager.waveActive = false;
        

        if (state == GameState.LOSS)
        {
            //lossUI function call in gameover
            gameOver.InitiateGameOver();
            return;
        }
        
        
        Debug.Log("wave over! build phase starting");
        state = GameState.BUILD;
        StartCoroutine(BuildBegin());
    }
}
