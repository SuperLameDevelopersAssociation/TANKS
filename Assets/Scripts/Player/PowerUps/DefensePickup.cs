using UnityEngine;
using System.Collections;
using TrueSync;

public class DefensePickup : MonoBehaviour {

    public int defenseMaxHealth = 100;

    Health playerHealth;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")   //Checks if collided with player
        {
            playerHealth = other.gameObject.GetComponent<Health>();

            if (playerHealth == null)
                Debug.LogError("There is no health script on " + other.gameObject.name);
            else
            {
                playerHealth.DefenseBoost(defenseMaxHealth);
                Destroy(this.gameObject);
            }
        }
    }
}
