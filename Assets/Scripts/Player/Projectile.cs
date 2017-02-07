using UnityEngine;
using TrueSync;

public class Projectile : TrueSyncBehaviour
{
    [HideInInspector]
    public FP speed = 15;           //Store speed for projectile
    [HideInInspector]
    public TSVector direction;      //Store the direction
    [AddTracking]
    private FP destroyTime = 3;     //Time before projectile is destroyed

    public int damage; //I am not using this variable yet but I put this here to make sure my Shooting script has this set up so when it is implemented it works.

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
            Health hitPlayer = other.gameObject.GetComponent<Health>();     //Reference the players movement script
            if (hitPlayer.owner != owner)   //Checks to see if the player hit is an enemy and not yourself
            {
                hitPlayer.TakeDamage(damage);
            }
        }
    }
}