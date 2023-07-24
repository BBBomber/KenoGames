using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class numberButtonScript : MonoBehaviour
{
    public Color notSelected; //1
    public Color selected; //2
    public Color matched; //3
    public Color notMatched; //4
    public Color DinoEgg; //5
    public Color Dino; //6

    public int state = 1;

    private Image image;

    private GameState gameState;

    private int ID;

    

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
         
        image.color = notSelected;

        ID = this.GetInstanceID() + 10;
        
    }

    private void OnMouseDown()
    {
        if (gameState.mouseEnabled)
        {
            gameState.gameManager.NumberClicked(ID, state);
            ChangeInitialState();
        }
        
        
        
    }

    private void ChangeInitialState()
    {
        
            if (state == 1)
            {
                state = 2;
                image.color = selected;

            }
            else if (state == 2)
            {
                state = 1;
                image.color = notSelected;
            }
        
        
    }


    public void ColourChange()
    {
        
        state = 1;
        image.color = notSelected;
    }


    public void MatchOrNot()
    {
        if (state == 1) 
        {
            state = 4;
            image.color = notMatched;
        }

        else if(state == 2)
        {
            state = 3;
            image.color = matched;
        }

        else if(state == 5)
        {
            state = 6;
            image.color = Dino;
        }
    }

    public void Reset()
    {
        if (state == 4 || state == 5 || state == 6) // Update the condition to include states 5 and 6
        {
            state = 1;
            image.color = notSelected;
            Debug.Log("dino egg reset"); // Add a debug log to indicate the reset of the dino egg
        }
        else if (state == 3)
        {
            state = 2;
            image.color = selected;
        }
    }

    public void SetEgg()
    {
        state = 5;
        image.color = DinoEgg;
    }

}
