using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameState {START, BUILD, RIDE, WIN, LOSS}

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private int startingCreditAmount = 40;
    public GameState state;

    public WaveManager waveManager;

    [Header("UI Vars")]
    public GameObject rideBtnUI;
    public GameObject shopPanelUI;
    public GameObject tutorialUI;


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

        //start with an amount of POP
        creditManager = creditManager.GetComponent<CreditManager>();

        creditManager.SetCredits(startingCreditAmount);

        tutorialUI.SetActive(true);

        shopPanelUI.SetActive(true);

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

        // Debug.Log("wave over! build phase starting");
        state = GameState.BUILD;
        StartCoroutine(BuildBegin());
    }

    public void GameWin()
    {
        gameOver.InitiateGameWin();
    }

    public void TutClose()
    {
        tutorialUI.SetActive(false);
    }

}
