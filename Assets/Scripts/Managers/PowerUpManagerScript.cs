using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TrueSync;

//christopher koester
public class PowerUpManagerScript : TrueSyncBehaviour
{
    public List<Transform> spawnLocations = new List<Transform>();
    [Tooltip("The amount of time between powerups")]
    public int respawnTime;
    private bool canSpawn = true; //this is bool used to determine whether or not an item is currently spawning. 0 is false 1 is true
    public GameObject[] prefabs;
    GameObject powerUp; //the curent spawned power up

    public override void OnSyncedStart()
    {
       //prefabs = (Resources.LoadAll("PowerUps")); //loading all prefabs from the resources/powerups folder
    }

    //Update is called once per frame
    public override void OnSyncedUpdate()
    {
        if (canSpawn && GameObject.FindWithTag("Powerup") == null)
        {
            TrueSyncManager.SyncedStartCoroutine(Respawn());
        }
    }
    //waiting to respawn an item (if no other item exists)
    IEnumerator Respawn()
    {//this function should be called each time after a player picks up a powerup
        canSpawn = false;
        //Iterate through the recources folder and choose from a random powerup
        powerUp = prefabs[TSRandom.Range(0, prefabs.Length)];
        //randomizing the spawn location for the powerup
        TSVector newPosition = spawnLocations[TSRandom.Range(0, spawnLocations.Count)].GetComponent<TSTransform>().position;
        //spawning the powerup
        TrueSyncManager.SyncedInstantiate(powerUp, newPosition, TSQuaternion.identity);
        yield return respawnTime;
        //setting the boolean, canSpawn, to true
        canSpawn = true;
    }
}
