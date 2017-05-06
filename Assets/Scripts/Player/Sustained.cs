using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Sustained : NetworkBehaviour
{
    public byte ID = 10;
    [HideInInspector]
    public int damage;
    [Tooltip("How often the sustained weapon does to the target")]
    public float damageFreq;

    private bool isWaiting = false;

    [ServerCallback]
    public void OnTriggerStay (Collider other)
    {
        if (!isWaiting)
        {
            if (other.gameObject.tag == "Player")   //Checks if collided with player
            {
                Health hitPlayer = other.gameObject.GetComponent<Health>();     //Reference the players movement script

                //Debug.LogError("HIt Player ID: " + hitPlayer.ID + " This ID: " + ID);

                if (hitPlayer.ID != ID)   //Checks to see if the player hit is an enemy and not yourself
                {
                    hitPlayer.RpcTakeDamage(damage, ID);
                    StartCoroutine(SendDamage());
                }
            }
        }
    }

    IEnumerator SendDamage()
    {
        isWaiting = true;
        yield return new WaitForSeconds(damageFreq);
        isWaiting = false;
    }

    public void OnTriggerExit(Collider other)
    {
        isWaiting = false;
    }
}