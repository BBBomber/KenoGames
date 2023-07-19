using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    // Start is called before the first frame update

    public GameManager gameManager;
    public payoutManager PayoutManager;
    public TokenValueDropdown tokenDropdown;

    public bool mouseEnabled = true;
    void Start()
    {
        
    }

    public void MouseSwitch()
    {
        if (mouseEnabled)
        {
            mouseEnabled = false;
        }
        else if (mouseEnabled == false)
        {
            mouseEnabled = true;
        }
    }

}
