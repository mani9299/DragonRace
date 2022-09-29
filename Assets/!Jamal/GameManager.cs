using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;

public class GameManager : MonoBehaviour
{
    
    public Text Announcertext;

    public static GameManager instance { get; private set; }

    public bool Greenenvironment;

    [Space]

    [Header("GameObjects")]

    public GameObject panel_Menu;
    public GameObject panel_HallOfDragons;
    public GameObject panel_ChangeName;
    public GameObject panel_RoomPanel;
    public GameObject panel_WalletConnecting;

    public GameObject parent_Dragons;

    [Space]

    [Header("Other")]

    public MenuScreen screen;
    public Animator anim_MainCam;
    public Animator anim_DragonCam;

    [Space]

    [Header("Scripts")]

    public Dragonslist dragonslist;

    [Space]

    [Header("UI")]

    public InputField input_RoomName;

    [Space]

    public Text text_Coins;
    public Text text_Wallet;

    [Space]

    public Button button_Play;
    public Image image_Connecting;

    [Header("Networking")]

    public NetworkRunner NetworkRunnerMenu;

    public enum MenuScreen
    {
        Menu,
        HallOfDragons,
        ChangeName,
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBG(0);
        text_Coins.text = PlayerPrefs.GetInt("Coins").ToString();
        //goto_Menu();        
    }

    IEnumerator Turnofftext()
    {
        yield return new WaitForSeconds(1f);
        Announcertext.enabled = false;
    }

    public void goto_RoomPanel()
    {
        DisableUI();
        panel_RoomPanel.SetActive(true);
    }
       
    public void quit()
    {
        Application.Quit();
    }

    #region UI Callbacks
    public void DisableUI()
    {
        panel_ChangeName.SetActive(false);
        panel_Menu.SetActive(false);
        panel_HallOfDragons.SetActive(false);
        panel_RoomPanel.SetActive(false);
        panel_WalletConnecting.SetActive(false);

        //Disable dragons List
        //parent_Dragons.SetActive(false);
    }

    public void goto_HallOfDragons()
    {
        DisableUI();
        panel_HallOfDragons.SetActive(true);
        panel_HallOfDragons.GetComponent<TweenCanvasGroupAlpha>().ResetAndPlay();
        parent_Dragons.SetActive(true);

        screen = MenuScreen.HallOfDragons;

        SoundManager.Instance.PlayOneShot(SoundManager.Instance.button_Menu);

        anim_MainCam.Play("mainCamBack", 0);
        anim_DragonCam.Play("DragonCamBack", 0);

        //dragonslist.SelectDragon(PlayerPrefs.GetInt("Selecteddragon"));
    }

    public void goto_ChangeName()
    {
        DisableUI();
        panel_ChangeName.SetActive(true);
        panel_ChangeName.GetComponent<TweenCanvasGroupAlpha>().ResetAndPlay();

        screen = MenuScreen.ChangeName;

        SoundManager.Instance.PlayOneShot(SoundManager.Instance.button_Menu);
    }

    public void goto_Menu()
    {
        DisableUI();
        panel_Menu.SetActive(true);
        panel_Menu.GetComponent<TweenCanvasGroupAlpha>().ResetAndPlay();

        if (screen == MenuScreen.HallOfDragons)
        {
            anim_MainCam.Play("MainCam", 0);
            anim_DragonCam.Play("DragonCamForward", 0);
        }

        screen = MenuScreen.Menu;        

        SoundManager.Instance.PlayOneShot(SoundManager.Instance.button_Menu);
    }


    #endregion

    #region Room panel

    public void LoadGame()
    {
        if (!string.IsNullOrEmpty(input_RoomName.text))
        {
            PlayerPrefs.SetString("RoomName", input_RoomName.text);
            //Destroy(NetworkRunnerMenu.gameObject);
            SceneManager.LoadScene(2);           
        }        
    }

    #endregion

    #region Network
    public void EnablePlayButton()
    {
        image_Connecting.gameObject.SetActive(false);
        button_Play.gameObject.SetActive(true);
    }

    #endregion
}


