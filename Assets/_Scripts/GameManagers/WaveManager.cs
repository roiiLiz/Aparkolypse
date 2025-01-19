using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Vars")]
    [SerializeField]
    public bool waveActive;
    private int waveIndex = -1;
    public Wave[] waves;
    public Transform[] laneIndicators;
    public GameObject[] enemiesLeft;
    public int totalEnemies = 1;
    public float remainingDeadline;
    public float totalDeadline;
    [SerializeField] private Image waveUI;

    [Header("Sound Vars")]
    
    //music

    public GameplayManager gameplayManager;

    [System.Serializable]
    public class Wave
    {
        public int laneAmount;
        public int enemyPopulation;
        public GameObject[] enemyTypes;
        //public float enemyWeight;
        public float spawnIntervals;
        public float waveDeadline;
    }

    private void OnEnable() { HealthComponent.OnDamageThresholdReached += ClearWave; }
    private void OnDisable() { HealthComponent.OnDamageThresholdReached -= ClearWave; }

    private void Start()
    {
        totalEnemies = waves[0].enemyPopulation;
    }

    private void Update()
    {
        if (waveActive)
        {
            enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy");
            totalEnemies = enemiesLeft.Length;

            if (waves[waveIndex].waveDeadline > 0)
            {
                waves[waveIndex].waveDeadline -= Time.deltaTime;
                //StartCoroutine(waveUpdate());
            }
            if (totalEnemies <= 0 && gameplayManager.state == GameState.RIDE)
            {
                
                StartCoroutine(RickeyClear());

            }
        }
    }

    private void ClearWave()
    {
        StartCoroutine(RickeyClear());
    }

    public IEnumerator WaveSpawn()
    {
        
        waveActive = true;
        waveIndex++;
        totalEnemies = waves[waveIndex].enemyPopulation;
        remainingDeadline = waves[waveIndex].waveDeadline;
        totalDeadline = waves[waveIndex].waveDeadline;
        StartCoroutine(waveUpdate());
        //waves[waveIndex].laneNumber = newlanestuff;
        waves[waveIndex].enemyPopulation = waves[waveIndex].enemyPopulation;

        for (int i = 0; i < waves[waveIndex].enemyPopulation; i++)
        {
            Debug.Log("index is " + waveIndex);
            //Debug.Log("enemy pop is " + waves[waveIndex].enemyPopulation);

            //weighted enemyspawn function (scrapping for now)

            //instantiate random enemies and choose specific lanes based on lane amount

            GameObject randomEnemy = waves[waveIndex].enemyTypes[Random.Range(0, waves[waveIndex].enemyTypes.Length)];

            int currentLane = Random.Range(0, waves[waveIndex].laneAmount);

            Instantiate(randomEnemy, laneIndicators[currentLane]);
            Debug.Log("lane is " + currentLane);

            //randomEnemy.transform.SetParent(laneIndicators[currentLane]);

            Debug.Log("enemy created was " + randomEnemy.name + " " + laneIndicators[0].position);
            yield return new WaitForSeconds(waves[waveIndex].spawnIntervals);

        }

        //StartCoroutine(waveUpdate());

        // StartCoroutine(RickeyClear());

        //begin music, or fade to new track
        //expose waveIndicator UI element

        //choose an increasing amount of lanes at random based on the wave number
        //laneAmount, lanes[] (random), enemyPopulation, enemyType, enemyTypeProbability, spawnIntervals (Random)
        //ui indicator for which lane they are spawning out of deactivated
        //then, instantiate enemies based on these wave rules at the set spawninterval ranges

    }

    public IEnumerator RickeyClear()
    {
        //rickey angry animation

        enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy");
        totalEnemies = enemiesLeft.Length;

        for (int i = 0; i < enemiesLeft.Length; i++)
        {
            Destroy(enemiesLeft[i]);
            totalEnemies--;
        }

        yield return new WaitForSeconds(1.5f);

        gameplayManager.RideEnd();

    }


    public IEnumerator waveUpdate()
    {
        Debug.Log("wave updating!");
        while (waveActive)
        {
            Debug.Log("wave updating while active!");

            //maybe per wave instead?? updates entire bar right now instead of in fifth segments
            waveUI.fillAmount = Mathf.InverseLerp(0, totalDeadline, remainingDeadline);
            remainingDeadline--;
            yield return new WaitForSeconds(1f);

            yield return null;
        }

        yield return null;

        //for (int i = 0; i < waves[waveIndex].waveDeadline; i++)
        //{
        //    if (waves[waveIndex].waveDeadline <= 0)
        //    {
        //        Debug.Log("rickey clar");
        //        StartCoroutine(RickeyClear());
        //    }
        //    else if (totalEnemies < 1)
        //    {
        //        Debug.Log("enemy clar");
        //        gameplayManager.GetComponent<GameplayManager>().RideEnd();
        //    }
        //}

        //update wave ui accordingly
        //yield return new WaitForSeconds(1f);


    }
    //wave start trigger is  vvv
    //on ui button press, sets waveactive to true
    //hide "ride" button
    //activate repair button
    //reveal wave start and ride phase ui elements
    //once the waveactive is true, wave UI and music will update, and rides will become able to attack
    //friendlyunit can check this script for waveactive as well for that?

    //while wave is active, check if the total amount of enemies has reached 0
    //if it has, waveactive, friendly coasters active, and ride phase are false, and build phase is true
    //shift corresponding UI elements

}
