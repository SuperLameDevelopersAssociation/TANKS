using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    public Text output;
    public Text matchTime;
    public int matchTimeInMinutes;
    public byte killsToWin;
    public int matchEndingTime;
    public float respawnTime;
    public List<Transform> spawnPoints;
    private static Dictionary<byte, GameObject> playerList = new Dictionary<byte, GameObject>();
    public List<byte> kills;
    public List<byte> deaths;

    [SyncVar]
    float minutes = 5;
    [SyncVar]
    float seconds = 0;

    bool isTie;
    bool deathmatchActive;
    [HideInInspector]
    public bool matchEnding;
    

    void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one GameManager in scene.");
        else
            instance = this;
    }

    void Start()
    {
        for (int i = 0; i < Prototype.NetworkLobby.LobbyManager.s_Singleton._playerNumber; i++)
        {
            kills.Add(0);
            deaths.Add(0);
        }
        deathmatchActive = true;
        minutes = matchTimeInMinutes;
        UpdateText();
    }

    void Update()
    {
        if (seconds <= 0)
        {
            minutes = minutes - 1;
            seconds = 59;
        }
        else if ((int)seconds >= 0)
        {
            seconds -= Time.deltaTime;
        }

        if (minutes <= 0 && seconds <= 0)
        {
            EndMatch();
        }

        UpdateTimerText();
    }

    public static void RegisterPlayer(byte ID, GameObject player)
    {
        if (!playerList.ContainsKey(ID))
        {
            playerList.Add(ID, player);
            player.GetComponent<PlayerSetup>().ID = ID;            
        }
    }

    public static void UnRegisterPlayer(byte ID)
    {
        playerList.Remove(ID);
    }

    public static GameObject GetPlayer(byte ID)
    {
        return playerList[ID];
    }

    public static GameObject[] GetAllPlayersArray()
    {
        return playerList.Values.ToArray();
    }

    public static List<GameObject> GetAllPlayersList()
    {
        return playerList.Values.ToList();
    }

    public void AwardPoints(byte murdererID, byte deadmanID)
    {
        kills[murdererID]++;
        deaths[deadmanID]++;
        UpdateText();

        if (deathmatchActive)
        {
            if (kills[murdererID] >= killsToWin)
            {
                EndMatch();
            }
        }
    }

    void UpdateText()
    {
        output.text = "";
        for (int index = 0; index < kills.Count; index++)
        {
            output.text += "Player: " + (index + 1) + ". Kills: " + kills[index] + " Deaths: " + deaths[index] + "\n";     
        }
    }

    void UpdateTimerText()
    {
        if (!matchEnding)
            matchTime.text = string.Format("{0:#00}:{1:00}", minutes, (int)seconds);
    }

    public void EndMatch()
    {
        if (!matchEnding)
        {
            matchEnding = true;

            byte player = PlayerThatWon();
            if (player != 100)
                matchTime.text = "Player " + (player + 1) + " Won!";
            else
                matchTime.text = "Tie!";

            SceneManager.LoadScene(0);
        }
    }

    public byte PlayerThatWon()
    {
        byte playerID = 0;
        byte lastAmt = 0;
        int killAmount = -1;

        for (byte i = 0; i < kills.Count; i++)
        {
            lastAmt = kills[i];
            if (lastAmt > killAmount)
            {
                playerID = i;
                killAmount = lastAmt;
                isTie = false;
            }

            else if (lastAmt == killAmount)
            {
                isTie = true;
            }
        }

        if(isTie)
            return 100; //This just means its a tie.

        return playerID;
    }
}