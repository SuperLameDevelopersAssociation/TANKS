using UnityEngine;
using System.Collections;
using TrueSync;

public class DamageBoost : MonoBehaviour {

    public double mulitplier;

    Health playerHealth;

    public void OnSyncedTriggerEnter(TSCollision other)
    {
        if (other.gameObject.tag == "Player")   //Checks if collided with player
        {
            playerHealth = other.gameObject.GetComponent<Health>();

            if (playerHealth == null)
                Debug.LogError("There is no shooting script on " + other.gameObject.name);
            else
            {
                //TrueSyncManager.SyncedStartCoroutine(playerHealth.GiveDamageBoost(mulitplier, ));
                TrueSyncManager.SyncedDestroy(this.gameObject);
            }
        }
    }
}
