using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIManager : MonoBehaviour
{
    public static APIManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance.gameObject);
        }
    }

    public Main main;

    [Space]

    public string post_Amount;
    public string post_User;
    public long post_SigTime;
    public string post_Sig;

    [Space]

    public PlayerDataBody playerData;

    // Start is called before the first frame update
    void Start()
    {
        ConvertCoinsToTOkens();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConvertCoinsToTOkens()
    {
        post_User = RaceSceneManager.instance.PlayerWallet;
        post_Amount = (RaceSceneManager.instance.Reward / 100).ToString();
        post_SigTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        post_Sig = "";
        playerData = new PlayerDataBody(post_User, post_Amount, post_SigTime, post_Sig);
        Debug.Log($"Player data to POST: {playerData}");
        main.UpdateValuesOnServer();
    }
}

[Serializable]
public class PlayerDataBody
{
    public string user;
    public string amount;
    public long sigTime;
    public string sig;

    public PlayerDataBody(string user, string amount, long sigTime, string sig)
    {
        this.user = user;
        this.amount = amount;
        this.sigTime = sigTime;
        this.sig = sig;
    }
}
