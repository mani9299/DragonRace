using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Dragonslist : MonoBehaviour
{
    [Header("GameObjects")]

    public GameObject[] dragons;

    [Space]

    [Header("Variables")]

    public int selecteddragon = 0;
    public float rotationSpeed;

    [Space]

    [Header("UI")]

    public Text text_Name;
    public Text text_Description;
    public Text text_Speed;
    public Text text_Power;

    public Button[] button_Dragon;

    [Space]

    [Header("Other")]

    public DragonStats[] dragonStats;

    [Space]

    [Header("Camera")]

    public Camera camera_Dragon;

    private void Start()
    {
        if(PlayerPrefs.GetInt("Selecteddragon") == 0)
        {
            PlayerPrefs.SetInt("Selecteddragon", 1);
        }
        SelectDragon(PlayerPrefs.GetInt("Selecteddragon") - 1);
        //RPC_SetKartId(selecteddragon);
    }

    public void Update()
    {
        if (Input.GetMouseButton(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            RotateObject();
        }
    }

    /// <summary>
    /// Rotate current selected UI Dragon
    /// </summary>
    void RotateObject()
    {
        float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;        
        // select the axis by which you want to rotate the GameObject
        dragons[selecteddragon].transform.Rotate(Vector3.down, XaxisRotation);
        
    }

    public void selectdragon()
    {
        PlayerPrefs.SetInt("Selecteddragon", selecteddragon);
        
    }

    public void nextdragon()
    {
        dragons[selecteddragon].SetActive(false);
        selecteddragon = (selecteddragon + 1) % dragons.Length;
        dragons[selecteddragon].SetActive(true);
    }



    public void Previousdragon()
    {
        dragons[selecteddragon].SetActive(false);
        selecteddragon--;
        if (selecteddragon < 0)
        {
            selecteddragon += dragons.Length;
        }
        dragons[selecteddragon].SetActive(true);
    }

    #region Dragon Selection
    /// <summary>
    /// Disable all dragon prefabs
    /// </summary>
    public void DisableDragons()
    {
        foreach (var dragon in dragons)
        {
            dragon.SetActive(false);
        }

        foreach (var button in button_Dragon)
        {
            //button.GetComponent<TweenScale>().ResetToBeginning();
        }
    }

    /// <summary>
    /// Set current select dragon to Selected
    /// </summary>
    /// <param name="selected"></param>
    public void SelectDragon(int selected)
    {
        DisableDragons();
        dragons[selected].SetActive(true);

        SetAttributes(selected);

        selecteddragon = selected;
        RPC_SetKartId(selecteddragon);

        //PlayerPrefs.SetInt("Selecteddragon", selected);

        if (GameManager.instance.screen == GameManager.MenuScreen.HallOfDragons)
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.button_DragonSelect);

    }

    #endregion

    #region UI Callbacks
    /// <summary>
    /// Set current selected dragon's attributes on UI
    /// </summary>
    /// <param name="dragon"></param>
    public void SetAttributes(int dragon)
    {
        text_Name.text = dragonStats[dragon].Key;
        text_Description.text = dragonStats[dragon].Description;
        text_Speed.text = dragonStats[dragon].Speed.ToString();
        text_Power.text = dragonStats[dragon].Power.ToString();
    }

    public void info_Dragons()
    {

    }

    public void info_Attributes()
    {

    }

    public void info_Skills()
    {

    }

    public void Domesticate()
    {

    }

    public void SpeedUp()
    {

    }

    #endregion
    /// <summary>
    /// RPC: Set current selected Dragon's ID across network
    /// </summary>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_SetKartId(int id)
    {     
        PlayerPrefs.SetInt("Selecteddragon", id + 1);
    }
}
