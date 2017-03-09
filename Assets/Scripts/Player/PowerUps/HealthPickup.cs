using UnityEngine;
using System.Collections;
using TrueSync;

public class HealthPickup : MonoBehaviour 
{
	Health playerHealth;

	public void OnSyncedTriggerEnter(TSCollision other)
	{
		if (other.gameObject.tag == "Player")   //Checks if collided with player
		{
			playerHealth = other.gameObject.GetComponent<Health>();

			if (playerHealth == null) //checks to see if the health script is attached
				Debug.LogError("There is no health script on " + other.gameObject.name);
			else if (playerHealth.currHealth != playerHealth.maxHealth)
			{
				playerHealth.currHealth += 15;

				if (playerHealth.currHealth > playerHealth.maxHealth) 
				{
					playerHealth.currHealth = playerHealth.maxHealth;
				}
				TrueSyncManager.SyncedDestroy(this.gameObject);
				playerHealth.SetHealthBar ();
			}
		}
	}
}
