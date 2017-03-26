using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnManager : NetworkBehaviour
{
    //[HideInInspector]
    public SyncList<GameObject> players;
    public List<GameObject> _players;
    public List<GameObject> spawnPoints;
    bool spawned;

    void Update()
    {
        if (isServer)
        {
            if (!spawned)
            {
                if (_players.Count < NetworkManager.singleton.numPlayers)
                {
                    RpcPopulatePlayerList();
                    CmdPopulatePlayerList();
                    _players = GameObject.FindGameObjectsWithTag("Player").ToList();
                }
                else if (_players.Count == NetworkManager.singleton.numPlayers)
                {
                    spawned = true;
                    for (int i = 0; i < _players.Count; i++)
                        players.Add(_players[i]);
                    CmdSpawnPlayers();
                }
            }
        }
    }

    [ClientRpc]
    void RpcPopulatePlayerList()
    {
        _players = GameObject.FindGameObjectsWithTag("Player").ToList();
    }

    [Command]
    void CmdPopulatePlayerList()
    {
        _players = GameObject.FindGameObjectsWithTag("Player").ToList();
    }

    [Command]
    void CmdSpawnPlayers()
    {
        for (byte i = 1; i <= players.Count; i++)
        {
            players[i - 1].GetComponent<PlayerSetup>().RpcSetID(i);
            RpcRespawn(i);
        }
    }

    //[Command]
    //void RpcSetPlayerID()
    //{
    //    Debug.LogError("Set id");
    //    player.GetComponent<PlayerSetup>().RpcSetID(ID);
    //    //Debug.LogError("Player: " + player.name + " ID: " + ID);
    //}

    [ClientRpc]
    public void RpcRespawn(byte playerID)
    {
        Vector3 spawnPos = spawnPoints[playerID - 1].transform.position;
        players[playerID - 1].transform.position = new Vector3(spawnPos.x, 0, spawnPos.z);
        players[playerID - 1].transform.rotation = Quaternion.identity;
        players[playerID - 1].GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}