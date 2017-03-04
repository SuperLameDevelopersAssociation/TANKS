using UnityEngine;
using System.Collections;
using TrueSync;

public class DamageBoost : MonoBehaviour {

    public double mulitplier;
    public int duration;

    Shooting playerShooting;

    public void OnSyncedTriggerEnter(TSCollision other)
    {
        if (other.gameObject.tag == "Player")   //Checks if collided with player
        {
            playerShooting = other.gameObject.GetComponent<Shooting>();

            if (playerShooting == null)
                Debug.LogError("There is no shooting script on " + other.gameObject.name);
            else
            {
                TrueSyncManager.SyncedStartCoroutine(playerShooting.GiveDamageBoost(mulitplier, duration));
                TrueSyncManager.SyncedDestroy(this.gameObject);
            }
        }
    }
}
