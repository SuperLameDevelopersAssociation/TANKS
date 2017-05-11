using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections.Generic;
using System.Collections;
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

    //public Text score;
    #region Variables
    //----------- Deathmatch ------------
    static byte staticKillsToWin = 3;
    [SyncVar]
    public byte killsToWin = 3;
    public List<byte> kills;
    public List<byte> deaths;

    //------------ TeamDeathmatch --------
    public int teamA_kills;
    public int teamB_kills;

    private List<int> teamA;
    private List<int> teamB;

    //-------------- Timer ---------------
    public GameObject score;
    [HideInInspector]
    public bool matchEnding;
    //public int matchTimeInMinutes;
    public float respawnTime;
    public Text results;
    public string matchTime = "0000";

    static float staticMinutes = 1;
    [SyncVar]
    float minutes = 1;
    [SyncVar]
    float seconds = 0;

    // ------------- Gameplay -------------
    public List<Transform> spawnPoints;
    static bool staticDeathmatchActive = true;
    static bool staticTeamDeathmatchActive  = false;
    [SyncVar]
    public bool deathmatchActive = true;
    [SyncVar]
    bool teamDeathmatchActive;

    private bool returnToMenu;
    private bool isTie;
    public GameObject endPanel;
    private static Dictionary<byte, string> playerNames = new Dictionary<byte, string>();
    private static Dictionary<byte, GameObject> playerList = new Dictionary<byte, GameObject>();
    private GameObject[] players;
    private Text namesText;
    private Text scoresText;
    private string baseScoreText;

    NetworkManager networkManager;
    #endregion

    private GameManager() { }

    public static bool IsInstanceNull()
    {
        return instance == null;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;

        teamDeathmatchActive = staticTeamDeathmatchActive;
        deathmatchActive = staticDeathmatchActive;
        minutes = staticMinutes;
        killsToWin = staticKillsToWin;

        teamA = new List<int>();
        teamB = new List<int>();

        for (int i = 0; i < Prototype.NetworkLobby.LobbyManager.s_Singleton._playerNumber; i++)
        {
            kills.Add(0);
            deaths.Add(0);

            if (teamDeathmatchActive)
            {
                if ((i + 3) % 2 == 1)
                {
                    teamA.Add(i);
                }
                else
                {
                    teamB.Add(i);
                }
            }
        }

        seconds = 1;
        namesText = score.transform.FindChild("PlayerNames").gameObject.GetComponent<Text>();
        scoresText = score.transform.FindChild("KillsDeaths").gameObject.GetComponent<Text>();
        
    }

    void Start()
    {
        StartCoroutine(AddPlayers());
        networkManager = NetworkManager.singleton;
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

        if (returnToMenu && Input.GetKeyDown(KeyCode.Space))
        {
            LoadMainMenu();
        }
    }

    IEnumerator AddPlayers()
    {
        yield return new WaitUntil(AllPlayersIn);

        foreach (GameObject player in players)
        {
            PlayerSetup info = player.GetComponent<PlayerSetup>();
            playerList.Add(info.ID, player);
            playerNames.Add(info.ID, info.playerName);
        }

        UpdateScoreText();

    }

    bool AllPlayersIn()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        return (players.Length == Prototype.NetworkLobby.LobbyManager.s_Singleton._playerNumber);
    }

    public static void SetGamemode(bool deathmatchVal, bool teamVal)
    {
        staticDeathmatchActive = deathmatchVal;
        staticTeamDeathmatchActive = teamVal;
    }

    public static void SetGameVars(int timer, byte score)
    {
        staticMinutes = timer;
        staticKillsToWin = score;
    }


    public static void RegisterPlayer(byte ID, GameObject player, string name, Color color, int tankSelected)
    {
        if (!playerList.ContainsKey(ID))
        {
            //playerList.Add(ID, player);
            player.GetComponent<PlayerSetup>().playerName = name;
            player.GetComponent<PlayerSetup>().tankColor = color;
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
        if (deathmatchActive)
        {
            kills[murdererID]++;
            deaths[deadmanID]++;
            if (kills[murdererID] >= killsToWin)
            {
                EndMatch();
            }
        }
        else if (teamDeathmatchActive)
        {
            if (teamA.Contains(murdererID) && !teamA.Contains(deadmanID))
            {
                kills[murdererID]++;
                deaths[deadmanID]++;
                teamA_kills++;
            }
            else if (teamB.Contains(murdererID) && !teamB.Contains(deadmanID))
            {
                kills[murdererID]++;
                deaths[deadmanID]++;
                teamB_kills++;
            }

            if (teamA_kills >= killsToWin || teamB_kills >= killsToWin)
            {
                EndMatch();
            }
        }

        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        namesText.text = "Player Names";
        scoresText.text = "Kills	/	Deaths";
        for (int index = 0; index < kills.Count; index++)
        {
            namesText.text += string.Format("\n{0} ", playerNames[(byte)index]); //"\nPlayer " + (index + 1) + " :";
            scoresText.text += string.Format("\n{0} \t\t\t {1}", kills[index], deaths[index]);
        }

        if (teamDeathmatchActive)
        {
            namesText.text += "\n ";
            scoresText.text += "\nTeam A: " + teamA_kills + "  Team B: " + teamB_kills + "\n";
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
            endPanel.SetActive(true);
            score.SetActive(false);

            if (deathmatchActive)
            {
                byte player = PlayerThatWon();
                if (player != 100)
                    results.text = "Player " + (player + 1) + " Won! \n";
                else
                    results.text = "Tie! \n";

                for (int index = 0; index < kills.Count; index++)
                {
                    results.text += "Player: " + (index + 1) + ". Kills: " + kills[index] + " Deaths: " + deaths[index] + "\n";
                }
            }

            if (teamDeathmatchActive)
            {

                string team = TeamThatWon();
                if (team != "Tie")
                {
                    results.text = "Team " + team + " Won! \n\n";
                }
                else
                    results.text = " Tie! \n";

                for (int index = 0; index < kills.Count; index++)
                {
                    results.text += "Player: " + (index + 1) + ". Kills: " + kills[index] + " Deaths: " + deaths[index] + "\n";
                }

                results.text += "\nTeam A: " + teamA_kills + "  Team B: " + teamB_kills + "\n";
            }

            results.text += "\n\n Press the Space bar to return to Main Menu. ";
            returnToMenu = true;
            Time.timeScale = 0;
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

    public string TeamThatWon()
    {
        if (teamA_kills > teamB_kills)
        {
            return "A";
        }
        else if (teamB_kills > teamA_kills)
        {
            return "B";
        }
        else
        {
            return "Tie";        //This just means its a tie.
        }
    }

    public void LoadMainMenu()
    {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }
}