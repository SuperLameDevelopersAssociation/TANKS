using UnityEngine;
using System.Collections;
using TrueSync;

public class HealthPickup : MonoBehaviour 
{
    public int gainedHealth = 15;

	Health playerHealth;

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")   //Checks if collided with player
		{
			playerHealth = other.gameObject.GetComponent<Health>();

			if (playerHealth == null) //checks to see if the health script is attached
				Debug.LogError("There is no health script on " + other.gameObject.name);
			else if (!playerHealth.isHealthFull()) //don't want to pick up the health if health if full
			{
                playerHealth.AddHealth(gainedHealth);
				Destroy(this.gameObject);
			}
		}
	}
}
