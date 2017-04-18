﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    #region Variables
    //----------- Deathmatch ------------
    public byte killsToWin;
    public List<byte> kills;
    public List<byte> deaths;

    //------------ TeamDeathmatch --------
    public byte teamKillsToWin;
    public int teamA_kills;
    public int teamB_kills;

    private List<int> teamA;
    private List<int> teamB;

    //-------------- Timer ---------------
    [HideInInspector]
    public bool matchEnding;
    public int matchTimeInMinutes;
    public float respawnTime;
    public Text output;
    public Text results;
    public string matchTime;
    public GameObject EndGamePanel;

    [SyncVar]
    float minutes = 5;
    [SyncVar]
    float seconds = 0;

    // ------------- Gameplay -------------
    public static GameManager instance;
    public List<Transform> spawnPoints;

    private bool isTie;
    public bool deathmatchActive;
    public bool teamDeathmatchActive;
    private static Dictionary<byte, GameObject> playerList = new Dictionary<byte, GameObject>();
    #endregion

    void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one GameManager in scene.");
        else
            instance = this;
    }

    void Start()
    {
        teamA = new List<int>();
        teamB = new List<int>();

        for (int i = 0; i < Prototype.NetworkLobby.LobbyManager.s_Singleton._playerNumber; i++)
        {
            kills.Add(0);
            deaths.Add(0);

            if ((i + 3) % 2 == 1)
            {
                teamA.Add(i);
                print(i + " goes to team A.");
            }
            else
            {
                teamB.Add(i);
                print(i + " goes to team B.");
            }
        }

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

            if (teamA_kills >= teamKillsToWin || teamB_kills >= teamKillsToWin)
            {
                EndMatch();
            }
        }
        UpdateText();
    }

    void UpdateText()
    {
        output.text = "";
        for (int index = 0; index < kills.Count; index++)
        {
            output.text += "Player: " + (index + 1) + ". Kills: " + kills[index] + " Deaths: " + deaths[index] + "\n";
        }

        if (teamDeathmatchActive)
        {
            output.text += "Team A: " + teamA_kills + "  Team B: " + teamB_kills + "\n";
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
            EndGamePanel.SetActive(true);

            if (deathmatchActive)
            {
                byte player = PlayerThatWon();
                if (player != 100)
                    output.text = "Player " + (player + 1) + " Won!";
                else
                    output.text = "Tie!";
            }

            if (teamDeathmatchActive)
            {
                string team = TeamThatWon();
                if (team != "Tie")
                {
                    output.text = "Team " + team + " Won!";
                }
                else
                    output.text = " Tie! ";

                LoadMainMenu();
            }
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

        if (isTie)
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
        SceneManager.LoadScene(0);
    }
}