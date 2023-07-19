using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayButton : MonoBehaviour
{
    
    
    public GameState gameState;
    public GameManager gameManager;
    public Button playButton;

    private void Start()
    {
        playButton.onClick.AddListener(StartGames);
    }


    public void StartGames()
    {
        if(gameState.PayoutManager.credits >= gameState.PayoutManager.betAmount)
        {
            if (gameState.mouseEnabled) 
            {
                if (gameManager != null)
                {





                    if (gameManager.selectedNumbersQueue.Count > 1)
                    {
                        // Call the method on the target script
                        gameManager.GameStart();
                    }


                }
            }
        }
        
        
       
    }


}
