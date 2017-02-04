using UnityEngine;
using System.Collections;
using TrueSync;

public class Projectile : TrueSyncBehaviour
{
    public FP speed = 15;           //Store speed for projectile
    public TSVector direction;      //Store the direction
    [AddTracking]
    private FP destroyTime = 3;     //Time before projectile is destroyed

    public override void OnSyncedUpdate()
    {
        if (destroyTime <= 0)       //Check if its time to destroy the projectile
        {
            TrueSyncManager.SyncedDestroy(this.gameObject); //Destroy gameobject
        }
        tsTransform.Translate(direction * speed * TrueSyncManager.DeltaTime);   //Move the projectile
        destroyTime -= TrueSyncManager.DeltaTime;   //Adjust the destroy time
    }

    public void OnSyncedTriggerEnter(TSCollision other)
    {
        if (other.gameObject.tag == "Player")   //Checks if collided with player
        {
            Movement hitPlayer = other.gameObject.GetComponent<Movement>();     //Reference the players movement script
            if (hitPlayer.owner != owner)   //Checks to see if the player hit is an enemy and not yourself
            {
                TrueSyncManager.SyncedDestroy(this.gameObject); //destroys bullet
                hitPlayer.Respawn();    //respawns player
            }
        }
    }
}