using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class payoutManager : MonoBehaviour
{

    public GameState gameState;
    private Dictionary<int, int[]> PayoutDictionary= new Dictionary<int, int[]>();
    
    private TextMeshProUGUI[] textArray = new TextMeshProUGUI[10];
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;
    public TextMeshProUGUI text4;
    public TextMeshProUGUI text5;
    public TextMeshProUGUI text6;
    public TextMeshProUGUI text7;
    public TextMeshProUGUI text8;
    public TextMeshProUGUI text9;
    public TextMeshProUGUI text10;

    public TextMeshProUGUI WinAmountText;
    public TextMeshProUGUI BetAmountText;
    public TextMeshProUGUI TokenAmountText;
    public TextMeshProUGUI Credits;


    public int betAmount;
    public int tokenAmount = 5;
    public int amountOfTokens = 1;
    public int maxAmountOfTokens = 500;
    private int winAmount = 0;
    public int credits = 100000;

    public Button decreaseButton;
    public Button increaseButton;

    private int[] EggMultipliers = new int[3];
    
    private void Awake()
    {
        PayoutDictionary.Add(0, new int[] { });
        PayoutDictionary.Add(1, new int[] {  });
        PayoutDictionary.Add(2,  new int[] { 0, 15 });
        PayoutDictionary.Add(3,  new int[] { 0, 2, 46 });
        PayoutDictionary.Add(4,  new int[] { 0, 2, 5, 91 });
        PayoutDictionary.Add(5,  new int[] { 0, 0, 3, 12, 810 });
        PayoutDictionary.Add(6,  new int[] { 0, 0, 3, 4, 70, 1600 });
        PayoutDictionary.Add(7,  new int[] { 0, 0, 1, 2, 21, 400, 7000 });
        PayoutDictionary.Add(8,  new int[] { 0, 0, 0, 2, 12, 98, 1652, 10000 });
        PayoutDictionary.Add(9,  new int[] { 0, 0, 0, 1, 6, 44, 335, 4700, 10000 });
        PayoutDictionary.Add(10, new int[] { 0, 0, 0, 0, 0, 5, 24, 142, 1000, 45000 });

        EggMultipliers[0] = 1;
        EggMultipliers[1] = 4;
        EggMultipliers[2] = 8;

        textArray[0] = text1;
        textArray[1] = text2;
        textArray[2] = text3;
        textArray[3] = text4;
        textArray[4] = text5;
        textArray[5] = text6;
        textArray[6] = text7;
        textArray[7] = text8;
        textArray[8] = text9;
        textArray[9] = text10;

    }
    private void Start()
    {
        betAmount = amountOfTokens * tokenAmount;
        SetBetAmountText(betAmount);
        SetCredits(credits);
        winAmount = 0;
        SetWinText(); 
        decreaseButton.onClick.AddListener(DecreaseTokenAmountText);
        increaseButton.onClick.AddListener(IncreaseTokenAmountText);
    }

    // Update is called once per frame


    public void DecidePayout(int SelectedAmount, int MatchedAmount, int HatchedEggs)
    {
        if(MatchedAmount < 0)
        {
            MatchedAmount= 0;
        }
        if(HatchedEggs < 0) { HatchedEggs= 0; }
        int multiplier;
        int[] temp = PayoutDictionary[SelectedAmount];
        multiplier = temp[MatchedAmount];
        winAmount= betAmount * multiplier;
        winAmount= EggMultipliers[HatchedEggs] * winAmount;
        credits += winAmount;
        SetWinText();
        SetCredits(credits);
        
    }

    public void SetText(int selectedAmount)
    {
        // Check if the selectedAmount exists in the PayoutDictionary
        if (PayoutDictionary.ContainsKey(selectedAmount))
        {
            int[] payoutArray = PayoutDictionary[selectedAmount];

            // Set the texts based on the payoutArray
            for (int i = 0; i < textArray.Length;)
            {
                if (i < payoutArray.Length)
                {
                    textArray[i].text = payoutArray[i].ToString();
                }
                else
                {
                    textArray[i].text = "-";
                }
                i++;
            }
        }
        else
        {
            Debug.LogWarning("Selected amount not found in the PayoutDictionary!");
        }
    }

    public void DecreaseTokenAmountText()
    {
        if(amountOfTokens > 1)
        {
            amountOfTokens--;
            betAmount = amountOfTokens * tokenAmount;
            TokenAmountText.text= amountOfTokens.ToString();
            SetBetAmountText(betAmount);
        }
    }

    public void IncreaseTokenAmountText()
    {
        if(amountOfTokens < maxAmountOfTokens)
        {
            amountOfTokens++;
            betAmount = amountOfTokens * tokenAmount;
            TokenAmountText.text = amountOfTokens.ToString();
            SetBetAmountText(betAmount);
        }
    }

    public void ResetTokenAmount()
    {
        amountOfTokens = 1;
        TokenAmountText.text = amountOfTokens.ToString();
        betAmount = amountOfTokens * tokenAmount;
        SetBetAmountText(betAmount);
    }

    public void SetBetAmountText(int cents)
    {

        float tempFloat = cents;
        float dollars = tempFloat / 100f;
        BetAmountText.text= "Bet Amount: " + dollars.ToString("F2") + "$";
    }

    public void SetCredits(int credits)
    {
        float tempFloat = credits;
        float credit = tempFloat / 100f;
        Credits.text= "Credits: " + credit.ToString("F2") + "$";
    }

    public void GameStart()
    {
        credits -= betAmount;
        SetCredits(credits);
        
    }
    private void SetWinText()
    {
        float tempFloat = winAmount;
        float win = tempFloat / 100f;
        WinAmountText.text= "Win Amount: " + win.ToString("F2") + "$";   
    }
}
