using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TrueSync;

public class SpawnManager : TrueSyncBehaviour
{
    [HideInInspector]
    public List<GameObject> players;
    public List<GameObject> spawnPoints;

    // Use this for initialization
    void Start () {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player);
        }

        SpawnPlayers();
    }
	
	void SpawnPlayers()
    {
        for(byte i = 1; i <= players.Count; i++)
        {
            Respawn(i);
        }
    }

    public void Respawn(byte playerID)
    {
        players[playerID - 1].GetComponent<TSTransform>().position = spawnPoints[playerID - 1].transform.position.ToTSVector();
        players[playerID - 1].GetComponent<TSTransform>().rotation = TSQuaternion.identity;
    }
}
