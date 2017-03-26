using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    public Text output;
    public float respawnTime;
    [HideInInspector]
    public List<Transform> spawnPoints;
    private static Dictionary<byte, GameObject> playerList = new Dictionary<byte, GameObject>();
    public List<byte> kills;
    public List<byte> deaths;
    byte killAmount;

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
        UpdateText();
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
        print("OUT murderer: " + murdererID);
        print("OUT ID: " + deadmanID);
        print(kills.Count);
        kills[murdererID]++;
        deaths[deadmanID]++;
        UpdateText();
    }

    [Client]
    void UpdateText()
    {
        output.text = "";
        for (int index = 0; index < kills.Capacity; index++)
        {
            output.text += "Player: " + (index + 1) + ". Kills: " + kills[index] + " Deaths: " + deaths[index] + "\n";
        }
    }
}