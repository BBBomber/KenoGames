using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class infoButton : MonoBehaviour
{
    public Button infoButtonn;
    public GameObject InfoScreenn;
    private bool Switch = true;

    private void Start()
    {
        infoButtonn.onClick.AddListener(InfoScreen);
    }

    private void InfoScreen()
    {
        if(Switch)
        {
            InfoScreenn.SetActive(true);
            Switch= false;
        }
        else
        {
            InfoScreenn.SetActive(false);
            Switch= true;
        }
    }
}
