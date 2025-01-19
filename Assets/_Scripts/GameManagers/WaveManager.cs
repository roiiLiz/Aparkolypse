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

    }

    public IEnumerator WaveSpawn()
    {
        waveIndex++;
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

        //clean up map and destroy all enemies
        //by tag????

        GameObject[] enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemiesLeft.Length; i++)
        {
            Destroy(enemiesLeft[i]);
        }

        //finanlly ride end
        gameplayManager.GetComponent<GameplayManager>().RideEnd();

        //begin music, or fade to new track
        //expose waveIndicator UI element
        
        //choose an increasing amount of lanes at random based on the wave number
        //laneAmount, lanes[] (random), enemyPopulation, enemyType, enemyTypeProbability, spawnIntervals (Random)
        //ui indicator for which lane they are spawning out of deactivated
        //then, instantiate enemies based on these wave rules at the set spawninterval ranges

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
