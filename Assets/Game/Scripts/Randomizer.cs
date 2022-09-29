using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Randomizer : MonoBehaviour
{
    public Dragon _defaultDragon;
    Dragon _newDragon;
    // Start is called before the first frame update
    void Start()
    {
         _newDragon = new Dragon(_defaultDragon);
        printData();
    }

    void printData()
    {
       

    }
}
