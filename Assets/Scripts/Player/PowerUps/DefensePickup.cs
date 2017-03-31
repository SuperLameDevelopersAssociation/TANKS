using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DefensePickup : NetworkBehaviour {

    public int defenseMaxHealth = 100;

    Health playerHealth;

    [Server]
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")   //Checks if collided with player
        {
            playerHealth = other.gameObject.GetComponent<Health>();

            if (playerHealth == null)
                Debug.LogError("There is no health script on " + other.gameObject.name);
            else
            {
                playerHealth.RpcDefenseBoost(defenseMaxHealth);
                NetworkServer.UnSpawn(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
