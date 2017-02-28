using UnityEngine;
using System.Collections;
using TrueSync;

public class Projectile : TrueSyncBehaviour
{
    [HideInInspector]
    public FP speed = 15;           //Store speed for projectile
    [HideInInspector]
    public Vector3 direction;       //Store the direction

    [HideInInspector]
    public int damage; 

    [HideInInspector]
    public TSVector actualDirection;

    public override void OnSyncedStart()
    {
        // actualDirection = new TSVector(direction.x, direction.y, direction.z);
    }

    void OnEnable()
    {
        if(Time.timeSinceLevelLoad > 1)
            TrueSyncManager.SyncedStartCoroutine(DestroyBullet(3));
    }

    public override void OnSyncedUpdate()
    {
        tsTransform.Translate(actualDirection * speed * TrueSyncManager.DeltaTime);   //Move the projectile
    }

    public void OnSyncedTriggerEnter(TSCollision other)
    {
        if (other.gameObject.tag == "Player")   //Checks if collided with player
        {
            Debug.LogError("You hit the object: " + other.gameObject.name);
            Health hitPlayer = other.gameObject.GetComponent<Health>();     //Reference the players movement script
            Debug.LogError("The owner of the hit player is: " + hitPlayer.owner.Name);
            Debug.LogError("The owner of the projectile is: " + owner.Name);
            if (hitPlayer.owner.Id != this.owner.Id)   //Checks to see if the player hit is an enemy and not yourself
            {
                Debug.LogError("In Trigger");
                hitPlayer.TakeDamage(damage, this.owner.Id);
                TrueSyncManager.SyncedStartCoroutine(DestroyBullet(0));
            }
        }
    }

    IEnumerator DestroyBullet(FP waitTime)
    {
        yield return waitTime;
        gameObject.SetActive(false);
    }
}