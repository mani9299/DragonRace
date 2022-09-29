using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_WEBGL
public class WebLogin : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Web3Connect();

    [DllImport("__Internal")]
    private static extern string ConnectAccount();

    [DllImport("__Internal")]
    private static extern void SetConnectAccount(string value);

    private int expirationTime;
    private string account; 

    void Start()
    {
        OnLogin();
    }

    public void OnLogin()
    {
        //Debug.Log(TrimWalletAddress("0x24734d58280D9e18571Ebb8383D87c9C3065e34D"));
        GameManager.instance.panel_WalletConnecting.SetActive(true);
        Web3Connect();
        OnConnected();
    }

    async private void OnConnected()
    {
        account = ConnectAccount();
        while (account == "") {
            await new WaitForSeconds(1f);
            account = ConnectAccount();
        };
        // save account for next scene
        string hiddenwWallet = TrimWalletAddress(account);
        PlayerPrefs.SetString("Account", hiddenwWallet);
        PlayerPrefs.SetString("UserWallet", account);
        GameManager.instance.goto_Menu();
        GameManager.instance.text_Wallet.text = $"Wallet: {account}";
        // reset login message
        SetConnectAccount("");
        // load next scene
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnSkip()
    {
        // burner account for skipped sign in screen
        PlayerPrefs.SetString("Account", "");
        // move to next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public string TrimWalletAddress(string address)
    {
        Debug.Log(address.Length);
        char[] first = address.ToCharArray(0, 4);
        char[] last = address.ToCharArray(address.Length - 4, 4);
       

        string initial = "";
        string middle = "***";
        string final = "";
        for(int i = 0; i < 4; i++)
        {
            initial += first[i];
            final += last[i];
        }
        return initial + middle + final;
    }
}
#endif
