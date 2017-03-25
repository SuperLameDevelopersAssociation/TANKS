using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TrueSync;

public class SpawnManager : TrueSyncBehaviour
{
    [HideInInspector]
    public List<GameObject> players;
    public List<GameObject> spawnPoints;

    public FP spawnDelay = 5;

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
            TrueSyncManager.SyncedStartCoroutine(Respawn(i, 0));
        }
    }
/*
    public void Respawn(byte playerID)
    {
        TSVector spawnPos = spawnPoints[playerID - 1].transform.position.ToTSVector();
        players[playerID - 1].GetComponent<TSTransform>().position = new TSVector(spawnPos.x, 0, spawnPos.z);
        players[playerID - 1].GetComponent<TSTransform>().rotation = TSQuaternion.identity;
        players[playerID - 1].GetComponent<TSRigidBody>().velocity = TSVector.zero;
        players[playerID - 1].GetComponent<Health>().inSpawn = true;
    }
*/
    public IEnumerator Respawn(byte playerID)
    {
        players[playerID - 1].GetComponent<Health>().inSpawn = true;
        TSVector spawnPos = spawnPoints[playerID - 1].transform.position.ToTSVector();
        players[playerID - 1].GetComponent<TSTransform>().position = new TSVector(spawnPos.x, 0, spawnPos.z);
        players[playerID - 1].GetComponent<TSTransform>().rotation = TSQuaternion.identity;
        players[playerID - 1].GetComponent<TSRigidBody>().velocity = TSVector.zero;
        players[playerID - 1].SetActive(false);
        yield return spawnDelay;
        players[playerID - 1].SetActive(true);
    }

    public IEnumerator Respawn(byte playerID, FP delay)
    {
        players[playerID - 1].GetComponent<Health>().inSpawn = true;
        TSVector spawnPos = spawnPoints[playerID - 1].transform.position.ToTSVector();
        players[playerID - 1].GetComponent<TSTransform>().position = new TSVector(spawnPos.x, 0, spawnPos.z);
        players[playerID - 1].GetComponent<TSTransform>().rotation = TSQuaternion.identity;
        players[playerID - 1].GetComponent<TSRigidBody>().velocity = TSVector.zero;
        yield return delay;
    }
}
