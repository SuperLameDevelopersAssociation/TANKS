using UnityEngine;
using System.Collections;
using TrueSync;

public class HealthPickup : MonoBehaviour 
{
	Health playerCurrHealth;

	public void OnSyncedTriggerEnter(TSCollision other)
	{
		if (other.gameObject.tag == "Player")   //Checks if collided with player
		{
			playerCurrHealth = other.gameObject.GetComponent<Health>();

			if (playerCurrHealth == null) //checks to see if the health script is attached
				Debug.LogError("There is no health script on " + other.gameObject.name);
			else
			{
				TrueSyncManager.SyncedDestroy(this.gameObject);
			}
		}
	}
}
