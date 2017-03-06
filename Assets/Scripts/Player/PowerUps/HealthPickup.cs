using UnityEngine;
using System.Collections;
using TrueSync;

public class HealthPickup : MonoBehaviour 
{
	Health currHealth;

	public void OnSyncedTriggerEnter(TSCollision other)
	{
		if (other.gameObject.tag == "Player") 
		{
			currHealth.gameObject.GetComponent<Health> ();

			if (currHealth == null) 
			{
				Debug.LogError ("The health script is not attached to " + other.gameObject.name);
			} 
			else 
			{
				TrueSyncManager.SyncedDestroy (this.gameObject);
				//playerHealth += 20;
			}
		}
	}
}
