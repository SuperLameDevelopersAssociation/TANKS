using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> players;
    public List<GameObject> spawnPoints;

    // Use this for initialization
    void Start()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player);
        }

        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        for (byte i = 1; i <= players.Count; i++)
        {
            Respawn(i);
        }
    }

    public void Respawn(byte playerID)
    {
        Vector3 spawnPos = spawnPoints[playerID - 1].transform.position;
        players[playerID - 1].transform.position = new Vector3(spawnPos.x, 0, spawnPos.z);
        players[playerID - 1].transform.rotation = Quaternion.identity;
        players[playerID - 1].GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}