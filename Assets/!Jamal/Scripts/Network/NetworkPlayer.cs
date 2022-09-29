using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Cinemachine;
using TMPro;
using Dreamteck.Splines;
using System;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static readonly List<NetworkPlayer> networkPlayers = new List<NetworkPlayer>();
    public static NetworkPlayer Local { get; set; }
    public static Action<NetworkPlayer> PlayerChanged;


    public SplineComputer Path;
    public TextMeshProUGUI playerNickNameTM;
    
    
    public Transform playerModel;

    //Network Variabels----------------------------------------------------------//
   
    [Networked(OnChanged = nameof(OnNickNameChanged))]  
    public NetworkString<_16> nickName { get; set; }         //Player Name

    [Networked(OnChanged = nameof(OnDragonIDChanged))]
    public int DragonId { get; set; }                        //Dragon ID

    [Networked(OnChanged = nameof(OnStartRaceChanged))]
    public NetworkBool StartRace { get; set; }               //Bool to check if both players have set their state to start race

    [Networked(OnChanged = nameof(OnEndRaceChanged))]
    public NetworkBool EndRace { get; set; }                    //Bool to check if the player reached end of race

    [Networked] public NetworkBool Entered { get; set; }     //Bool to check if player has entered the room/match

    [Networked] public int StartRaceTick { get; set; }

    [Networked] public int EndRaceTick { get; set; }

    [Networked] public int Token { get; set; }

    //[Networked] public float SplinePercentage => GetComponent<SplineFollower>().;


    //---------------------------------------------------------------------------//
    public bool IsLeader => Object != null && Object.IsValid && Object.HasStateAuthority;

    bool isPublicJoinMessageSent = false;

    
    public DragonLoad dragonLoad;
    

    //public LocalCameraHandler localCameraHandler;
    public GameObject localUI;

    //Other components
    //NetworkInGameMessages networkInGameMessages;

    void Awake()
    {
       // networkInGameMessages = GetComponent<NetworkInGameMessages>();
    }
    void Start()
    {
        
    }

    #region Fusion
    public override void Spawned()
    {
        base.Spawned();
        
        if (Object.HasInputAuthority)
        {
            Local = this;
            Camera.main.gameObject.SetActive(false);
            //PlayerPrefs.SetString("PlayerNickname", "Firefist");
            //RPC_SetNickName("Player" + UnityEngine.Random.Range(0, 1000).ToString());
            RPC_SetNickName(PlayerPrefs.GetString("Account"));
            Debug.Log($"Selected Dragon {PlayerPrefs.GetInt("Selecteddragon")}");
            NetworkPlayer.Local.RPC_SetDragonID(PlayerPrefs.GetInt("Selecteddragon"));     

            Debug.Log("Spawned local player with ID: " + DragonId);

            //Path
            //Path = GameObject.FindGameObjectWithTag("Path").GetComponent<SplineComputer>();
            //GameObject.FindGameObjectWithTag("Player").GetComponent<SplineFollower>().spline = Path;

            //my code-----------------------------
            //GetComponent<SplineFollower>().spline = Path;
            //------------------------------------
            //gameObject.tag = "Untagged";
            //Path.tag = "Untagged";
        }
        else
        {
            Camera localcamera = GetComponentInChildren<Camera>();
            localcamera.enabled = false;

            CinemachineFreeLook Vlocalcamera = GetComponentInChildren<CinemachineFreeLook>();
            Vlocalcamera.enabled = false;

            AudioListener audiolis = GetComponentInChildren<AudioListener>();
            audiolis.enabled = false;
            localcamera.gameObject.SetActive(false);

            //Path 

            //Path = GameObject.FindGameObjectWithTag("Path2").GetComponent<SplineComputer>();
            //GameObject.FindGameObjectWithTag("Player").GetComponent<SplineFollower>().spline = Path;

            //My code--------------------------
            //GetComponent<SplineFollower>().spline = Path;
            //---------------------------------

            //Path.tag = "Untagged";

            Debug.Log("Spawned remote player with ID: " + DragonId);

            //Set the player as player object
            Runner.SetPlayerObject(Object.InputAuthority, Object);
        }

        this.Entered = true;

        Debug.Log("Local Player: " + Runner.LocalPlayer.PlayerId);

        Debug.Log("Network players length: " + NetworkPlayer.networkPlayers.Count);

        if (networkPlayers.Contains(this)) return;

        NetworkPlayer.networkPlayers.Add(this);

        Debug.Log("Network players length: " + NetworkPlayer.networkPlayers.Count);

        
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
        {
            var netPlayer = Object.GetComponent<NetworkPlayer>();
            Debug.Log($"OnPlayerLeft networkPlayer index: " + networkPlayers.IndexOf(netPlayer));
            RaceSceneManager.instance.UpdatePlayerList(netPlayer, networkPlayers.IndexOf(netPlayer), false, true);
            Debug.Log($"Despawned: {netPlayer}");
            networkPlayers.Remove(netPlayer);
            Runner.Despawn(Object);
        }

        
    }
    #endregion

    #region Dragon Name
    static void OnNickNameChanged(Changed<NetworkPlayer> changed)
    {
        Debug.Log($"{Time.time} OnHPChanged value {changed.Behaviour.nickName}");

        changed.Behaviour.OnNickNameChanged();
    }

    private void OnNickNameChanged()
    {
        Debug.Log($"Nickname changed for player to {nickName} for player {gameObject.name}");

        playerNickNameTM.text = nickName.ToString();

        name = nickName.ToString();

        RaceSceneManager.instance.UpdatePlayerList(this, networkPlayers.IndexOf(this) + 1, false, false);

        RaceSceneManager.instance.GameLog($"Player[{name}] joined");
    }

    /// <summary>
    /// Players Nickname RPC to sync it across the network
    /// </summary>
    /// <param name="nickName"></param>
    /// <param name="info"></param>
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetNickName(string nickName, RpcInfo info = default)
    {
        Debug.Log($"[RPC] SetNickName {nickName}");
        this.nickName = nickName;

        if (!isPublicJoinMessageSent)
        {
           // networkInGameMessages.SendInGameRPCMessage(nickName, "joined");

            isPublicJoinMessageSent = true;
        }
    }
    #endregion

    #region Dragon ID
    static void OnDragonIDChanged(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.OnDragonIDChanged();
    }

    private void OnDragonIDChanged()
    {
        //foreach (var networkPlayer in NetworkPlayer.networkPlayers)
        //{
        //    Debug.Log("Network player ID: " + networkPlayer.DragonId);
        //    Debug.Log("Network player name: " + networkPlayer.nickName);
        //    networkPlayer.dragonLoad.LoadDragon(networkPlayer.DragonId);
        //}

        dragonLoad.LoadDragon(DragonId);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetDragonID(int id, RpcInfo info = default)
    {
        Debug.Log("Inside dragon ID RPC: " + id);
        this.DragonId = id;       
    }
    #endregion

    #region Start Race Check
    static void OnStartRaceChanged(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.OnStartRaceChanged();
    }

    private void OnStartRaceChanged()
    {
        GetComponent<CharacterInputHandler>().CheckForRaceStart();
        RaceSceneManager.instance.UpdatePlayerList(this, networkPlayers.IndexOf(this), true, false);
    }

    /// <summary>
    /// Player's Ready state RPC to sync across the network
    /// </summary>
    /// <param name="networkBool"></param>
    /// <param name="info"></param>
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetStartRace(NetworkBool networkBool, RpcInfo info = default)
    {
        this.StartRace = networkBool;
    }

    #endregion

    #region End Race Check
    static void OnEndRaceChanged(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.OnEndRaceChanged();
    }

    private void OnEndRaceChanged()
    {
        RaceSceneManager.instance.CheckLeaveButton();
    }

    /// <summary>
    /// Player's end state RPC to sync across netowrk
    /// </summary>
    /// <param name="networkBool"></param>
    /// <param name="info"></param>
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetEndRace(NetworkBool networkBool, RpcInfo info = default)
    {
        this.EndRace = networkBool;
    }

    #endregion

    #region Reward
    public void RewardUser(int ammount)
    {
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + ammount);
    }

    #endregion

    #region Race
    /// <summary>
    /// Called when dragon/player reaches the end of race/spline
    /// </summary>
    public void OnReachedEnd()
    {
        EndRaceTick = Runner.Simulation.Tick;        

        Debug.Log($"{gameObject.name} EndRaceTick => {EndRaceTick}");
        Debug.Log( $"{gameObject.name} Reached End with {GetTotalRaceTime()}secs");

        dragonLoad.currentDragon.GetComponent<Animator>().SetBool("Fly", false);

        if (Local)
        {
            Debug.Log("Race End RPC from Local");
            RPC_SetEndRace(true);
        }        
        RaceSceneManager.instance.PlayerReachedEnd(this);      
    }

    /// <summary>
    /// Returns the total time we have been racing for, in seconds.
    /// </summary>
    /// <returns></returns>
    public float GetTotalRaceTime()
    {
        if (!Runner.IsRunning || StartRaceTick == 0)
            return 0f;

        var endTick = EndRaceTick == 0 ? Runner.Simulation.Tick.Raw : EndRaceTick;
        return TickHelper.TickToSeconds(Runner, endTick - StartRaceTick);
    }

    /// <summary>
    /// Called when race has started, to set all the values and references
    /// </summary>
    public void RaceStarted(SplineComputer path)
    {
        StartRaceTick = Runner.Simulation.Tick;
        Debug.Log($"{gameObject.name} StartRaceTick => {StartRaceTick}");
        dragonLoad.currentDragon.GetComponent<Animator>().SetBool("Fly", true);
        
        GetComponent<SplineFollower>().spline = path;
        GetComponent<SplineFollower>().follow = true;
    }
    #endregion
}
