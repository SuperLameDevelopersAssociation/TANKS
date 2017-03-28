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
    public GameObject[] prefabs;
    GameObject powerUp; //the curent spawned power up
    GameObject currentPowerUp;

    //Update is called once per frame
    void Update()
    {
        if (canSpawn && currentPowerUp == null)
        {
            canSpawn = false;
            Invoke("CmdPickupRespawn", respawnTime);
        }
    }

    //waiting to respawn an item (if no other item exists)
    [Command]
    void CmdPickupRespawn()
    {//this function should be called each time after a player picks up a powerup
        //canSpawn = false;
        //Iterate through the recources folder and choose from a random powerup
        powerUp = prefabs[Random.Range(0, prefabs.Length)];
        // since it is checking if this exists on update, best to be defensive and make sure it is here
        powerUp.tag.Equals("Powerup"); 
        //randomizing the spawn location for the powerup
        Vector3 newPosition = spawnLocations[Random.Range(0, spawnLocations.Count)].position;
        //spawning the powerup
        currentPowerUp = NetworkManager.Instantiate(powerUp, newPosition, Quaternion.identity) as GameObject;
        //setting the boolean, canSpawn, to true
        canSpawn = true;
    }
}
