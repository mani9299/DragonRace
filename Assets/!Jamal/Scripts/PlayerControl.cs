using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using UnityEngine.UI;
using System;

public class PlayerControl : MonoBehaviour
{

    public GameObject button;


    public void Setsplines()
    {
        CharacterInputHandler.instance.isreadytogo = true;
        //my code-------------------------------------
        //CharacterInputHandler.instance.GetNetworkInput();
        //--------------------------------------------

        button.SetActive(false);
    }
}
