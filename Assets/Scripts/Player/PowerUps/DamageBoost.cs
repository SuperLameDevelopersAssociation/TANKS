using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DamageBoost : NetworkBehaviour {

    public double mulitplier;
    public int duration;

    Shooting playerShooting;

    [Server]
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")   //Checks if collided with player
        {
            playerShooting = other.gameObject.GetComponent<Shooting>();

            if (playerShooting == null)
                Debug.LogError("There is no shooting script on " + other.gameObject.name);
            else
            {
                StartCoroutine(playerShooting.GiveDamageBoost(mulitplier, duration));
                Destroy(gameObject);
            }
        }
    }
}
