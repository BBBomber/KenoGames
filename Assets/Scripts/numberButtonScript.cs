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

    private int state = 1;

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
    }

    public void Reset()
    {
        if (state == 4)
        {
            state = 1;
            image.color = notSelected;
        }

        if (state == 3)
        {
            state = 2;
            image.color = selected;
        }
    }

    

}
