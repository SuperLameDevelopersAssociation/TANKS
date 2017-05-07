using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class PowerUpManagerScript : NetworkBehaviour
{
    public List<Transform> spawnLocations = new List<Transform>();
    [Tooltip("The amount of time between powerups")]
    public int respawnTime;
    private bool canSpawn = true; //this is bool used to determine whether or not an item is currently spawning.
    public GameObject[] pickupPrefabs;
    public GameObject bombPrefab;
    GameObject powerUp; //the curent spawned power up
    GameObject currentPowerUp;

    //Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            canSpawn = false;
            Invoke("CmdPickupRespawn", respawnTime);
        }
    }

    //waiting to respawn an item (if no other item exists)
    //this function should be called each time after a player picks up a powerup
    [Server]
    void CmdPickupRespawn()         
    {
        Vector3 newPosition = spawnLocations[Random.Range(0, spawnLocations.Count)].position;                       //randomizing the spawn location for the powerup

        if (currentPowerUp == null)
        {
            powerUp = pickupPrefabs[Random.Range(0, pickupPrefabs.Length)];                                             //pick a random powerup    
            currentPowerUp = NetworkManager.Instantiate(powerUp, newPosition, Quaternion.identity) as GameObject;       //spawning the powerup
            NetworkServer.Spawn(currentPowerUp);
        }
        else
        {
            //GameObject tempBomb = NetworkManager.Instantiate(bombPrefab, newPosition, Quaternion.identity) as GameObject;       //spawning the powerup
            //NetworkServer.Spawn(tempBomb);
        }
        canSpawn = true;
    }
}
