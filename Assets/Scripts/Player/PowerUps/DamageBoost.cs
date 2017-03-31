using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DamageBoost : NetworkBehaviour {

    public float mulitplier;
    public int duration;

    Shooting playerShooting;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")   //Checks if collided with player
        {
            playerShooting = other.gameObject.GetComponent<Shooting>();

            if (playerShooting == null)
                Debug.LogError("There is no shooting script on " + other.gameObject.name);
            else
            {
                playerShooting.CmdGiveDamageBoost(mulitplier, duration);
                NetworkServer.UnSpawn(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
