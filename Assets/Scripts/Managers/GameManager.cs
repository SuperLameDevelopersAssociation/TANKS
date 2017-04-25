using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    private static GameManager instance;
    public static GameManager GetInstance
    {
        get
        {
            return instance;
        }
    }

    public GameObject score;
    public int matchTimeInMinutes;
    public byte killsToWin;
    public int matchEndingTime;
    public float respawnTime;
    public List<Transform> spawnPoints;
    private static Dictionary<byte, GameObject> playerList = new Dictionary<byte, GameObject>();
    public List<byte> kills;
    public List<byte> deaths;

    int minutes = 5;
    [SyncVar]
    float seconds = 0;

    bool isTie;
    bool deathmatchActive;
    [HideInInspector]
    public bool matchEnding;

    public string matchTime = "";

    private Text names;
    private Text scores;
    private GameManager() { }

    public static bool isInstanceNull()
    {
        return instance != null;
    }

    void Awake()
    {
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
        seconds = 1;
        names = score.transform.FindChild("PlayerNames").gameObject.GetComponent<Text>();
        scores = score.transform.FindChild("KillsDeaths").gameObject.GetComponent<Text>();
        UpdateScoreText();
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

    public static void RegisterPlayer(byte ID, GameObject player, int tankSelected)
    {
        if (!playerList.ContainsKey(ID))
        {
            playerList.Add(ID, player);
            player.GetComponent<PlayerSetup>().ID = ID;
            player.GetComponent<PlayerSetup>().tankSelected = tankSelected;
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
        UpdateScoreText();

        if (deathmatchActive)
        {
            if (kills[murdererID] >= killsToWin)
            {
                EndMatch();
            }
        }
    }

    void UpdateScoreText()
    {
        //names.text = "";
        //scores.text = "";
        for (int index = 0; index < kills.Count; index++)
        {
            // Get typed in name.
            string playerName = "Player";
            names.text += string.Format("\n{0}", " Player " + (index + 1));
            scores.text += string.Format("\n{0} \t\t\t {1}", kills[index], deaths[index]);
        //score.text += " Name " + (index + 1) + ". Kills: " + kills[index] + " Deaths: " + deaths[index] + "\n";     
    }

    }

    void UpdateTimerText()
    {
        if (!matchEnding)
            matchTime = string.Format("{0:#00}{1:00}", minutes, (int)seconds);
    }

    public void EndMatch()
    {
        if (!matchEnding)
        {
            matchEnding = true;

            byte player = PlayerThatWon();
            scores.text = "";
            if (player != 100)
                names.text = "Player " + (player + 1) + " Won!";
            else
                names.text = "Tie!";

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