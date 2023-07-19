using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Collider : MonoBehaviour
{
    public Collider2D button1;
    private GameState gameState;

    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
        button1 = GetComponentInChildren<Collider2D>();
    }
    private void OnMouseDown()
    {
        gameState.gameManager.ResetGame();
        
        
    }
}
