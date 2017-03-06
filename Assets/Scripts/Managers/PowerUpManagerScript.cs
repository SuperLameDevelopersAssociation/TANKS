using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TrueSync;

//christopher koester
public class PowerUpManagerScript : TrueSyncBehaviour
{
    public List<Transform> spawnLocations = new List<Transform>();
    public FP respawnTime;
    private byte canSpawn = 1; //this is bool used for whether or not an item is currently spawning. 0 is false 1 is true
    public Object[] prefabs;
    GameObject powerUp; //the curent spawned power up

    public override void OnSyncedStart()
    {
       prefabs = (Resources.LoadAll("PowerUps")); //loading all prefabs from the resources/powerups folder
    }

    //Update is called once per frame
    public override void OnSyncedUpdate()
    {
        if (canSpawn == 1)
        {
            TrueSyncManager.SyncedStartCoroutine(Respawn());
        }
    }
    //waiting to respawn an item (if no other item exists)
    IEnumerator Respawn()
    {//this function should be called each time after a player picks up a powerup
        canSpawn = 0;
        if (powerUp == null)
        {
            //Iterate through the recources folder and choose from a random powerup
            powerUp = (GameObject)prefabs[Random.Range(0, prefabs.Length)];
            //spawning the powerup
            Instantiate(powerUp);
            print(powerUp.GetComponent<TSTransform>().position);
            //randomizing the spawn location for the powerup
            powerUp.GetComponent<TSTransform>().position = spawnLocations[Random.Range(0, spawnLocations.Count)].transform.position.ToTSVector();
            print(powerUp.GetComponent<TSTransform>().position);
        }
        yield return respawnTime;
        //setting the boolean, canSpawn, to true
        canSpawn = 1;
    }
}
