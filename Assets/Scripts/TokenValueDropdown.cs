using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TokenValueDropdown : MonoBehaviour
{
    public payoutManager PayoutManager;
    public TMP_Dropdown m_Dropdown;
    private int tokenAmount; // Variable to store the token amount in cents
    private int maxAmountOfTokens = 500; //variable to store the current max amount of tokens


    void Start()
    {
        // Fetch the Dropdown GameObject

        // Add listener for when the value of the Dropdown changes, to take action
        m_Dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(m_Dropdown);
        });
    }

    // Update the tokenAmount variable with the selected value in cents
    void DropdownValueChanged(TMP_Dropdown change)
    {
        // Convert the selected token value to cents and store it in the tokenAmount variable
        switch (change.value)
        {
            case 0:
                tokenAmount = 5; // 5 cents
                maxAmountOfTokens = 500;
                break;
            case 1:
                tokenAmount = 50; // 50 cents
                maxAmountOfTokens= 50;
                break;
            case 2:
                tokenAmount = 100; // 1 dollar
                maxAmountOfTokens= 25;
                break;
            case 3:
                tokenAmount = 500; // 5 dollars
                maxAmountOfTokens = 5;
                break;
            default:
                tokenAmount = 0; // Default value if none of the cases match
                break;
        }
        PayoutManager.tokenAmount= tokenAmount;
        PayoutManager.maxAmountOfTokens= maxAmountOfTokens;
        PayoutManager.ResetTokenAmount();
    }
}