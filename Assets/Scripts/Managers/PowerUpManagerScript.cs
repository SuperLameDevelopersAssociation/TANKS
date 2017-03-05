using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TrueSync;

//christopher koester
public class PowerUpManagerScript : TrueSyncBehaviour
{
public List<Transform> powerupLocations = new List<Transform>();
    public FP respawnTime;
    private byte canSpawn = 1; //this is bool used for whether or not an item is currently spawning. 0 is false 1 is true
    Object[] prefabs;
    GameObject powerUp; //the curent spawned power up


    public override void OnSyncedStart()
    {
        prefabs = Resources.LoadAll("Resources/PowerUps"); //loading all prefabs from the resources/powerups folder
    }

    //Update is called once per frame
    public override void OnSyncedUpdate()
    {
        if (canSpawn == 1)
        {
            print("is true");
            TrueSyncManager.SyncedStartCoroutine(Respawn());
        }
    }
    //waiting to respawn an item (if no other item exists)
    IEnumerator Respawn()
    {//this function should be called each time after a player picks up a powerup
        canSpawn = 0;
        print("is waiting");
        if (powerUp == null)
        {
            print(prefabs = Resources.LoadAll("Resources/PowerUps"));
            //Iterate through the recources folder and spawn a random powerup
            powerUp = (GameObject)prefabs[Random.Range(0, prefabs.Length)];
            //spawning the powerup
            GameObject spawn = Instantiate(powerUp);
            print("spawned");
        }
        yield return respawnTime;
        canSpawn = 1;
        print("done waiting");
    }
}
