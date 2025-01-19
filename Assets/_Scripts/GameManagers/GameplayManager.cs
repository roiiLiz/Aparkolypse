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

    private void Start()
    {
        state = GameState.START;
        StartCoroutine(BuildBegin());
    }

    public IEnumerator BuildBegin()
    {
        //units unable to attack

        //pause POP generation

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
        
        //resume POP generation

        //make grid unselectable

        //units able to attack

        //check castle HP at the end of round
        //if 0, set state to LOSS

    }

    public void RideEnd()
    {
        waveManager.waveActive = false;
        if (state == GameState.LOSS)
        {
            //lossUI function call in gameover

            return;
        }
        
        
        Debug.Log("wave over! build phase starting");
        state = GameState.BUILD;
        BuildBegin();
    }
}
