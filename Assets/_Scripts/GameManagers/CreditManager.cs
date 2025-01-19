using System;
using UnityEngine;

public class CreditManager : MonoBehaviour
{
    [Header("Passive Credit Generation")]
    [SerializeField]
    private int creditsGenerated;
    [SerializeField]
    private float timeBetweenCredits;

    private int totalCredits;
    public int TotalCredits { get { return totalCredits; } }

    private float time;
    
    public static event Action<int> OnCreditsUpdate;


    public GameplayManager gameplayManager;
    private void OnEnable()
    {
        EnemyDeathComponent.grantDeathCredits += AddCredits;
        GridSelector.OnBuyUnit += SubtractCredits;
    }

    private void OnDisable()
    {
        EnemyDeathComponent.grantDeathCredits -= AddCredits;
        GridSelector.OnBuyUnit += SubtractCredits;
    }

    private void Start()
    {
        //SetCredits(0);
    }

    private void Update()
    {
        if (time < timeBetweenCredits)
        {
            time += Time.deltaTime;
        } else if (gameplayManager.GetComponent<GameplayManager>().state == GameState.RIDE)
        {
            time = 0f;
            AddCredits(creditsGenerated);
        }
    }

    public void SetCredits(int creditAmount)
    {
        totalCredits = creditAmount;
        OnCreditsUpdate?.Invoke(totalCredits);
    }

    public void AddCredits(int creditsToAdd)
    {
        totalCredits += creditsToAdd;
        OnCreditsUpdate?.Invoke(totalCredits);
    }

    public void SubtractCredits(int creditsToSubtract)
    {
        totalCredits -= creditsToSubtract;
        OnCreditsUpdate?.Invoke(totalCredits);
    }


}