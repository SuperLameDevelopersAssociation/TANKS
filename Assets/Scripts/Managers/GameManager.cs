using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    public float respawnTime;
    [HideInInspector]
    public List<Transform> spawnPoints;
    private static Dictionary<byte, GameObject> playerList = new Dictionary<byte, GameObject>();


    void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one GameManager in scene.");
        else
            instance = this;
    }

    public static void RegisterPlayer(byte ID, GameObject player)
    {
        playerList.Add(ID, player);
        player.GetComponent<PlayerSetup>().ID = ID;
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
}