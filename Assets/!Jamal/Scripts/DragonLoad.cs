using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonLoad : MonoBehaviour
{

    public GameObject[] Dragons;
    [HideInInspector] public GameObject currentDragon;

    // Start is called before the first frame update
    void Start()
    {
        //int selecteddragon = PlayerPrefs.GetInt("Selecteddragon");
        //GameObject prefab = Dragons[selecteddragon];
        //Dragons[selecteddragon].SetActive(true);
    }


    public void LoadDragon(int id)
    {
        foreach (var dragon in Dragons)
        {
            dragon.SetActive(false);
        }
        Dragons[id - 1].SetActive(true);
        Debug.Log("dragon Playerpref: " + PlayerPrefs.GetInt("Selecteddragon"));
        currentDragon = Dragons[id - 1];
        //Disable Loading UI
        if (RaceSceneManager.instance.panel_Loadingimage.activeInHierarchy)
        {
            Debug.Log("Disable loader");
            RaceSceneManager.instance.panel_Loadingimage.SetActive(false);
            RaceSceneManager.instance.panel_StartRace.SetActive(true);
        }

    }

}
