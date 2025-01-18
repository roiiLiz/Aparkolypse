using System;
using TMPro;
using UnityEngine;

public class CreditTracker : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    private void OnEnable() { CreditManager.OnCreditsUpdate += UpdateCreditText; }
    private void OnDisable() { CreditManager.OnCreditsUpdate -= UpdateCreditText; }

    private void UpdateCreditText(int credits)
    {
        text.text = $"Total Credits: {credits}";
    }
}