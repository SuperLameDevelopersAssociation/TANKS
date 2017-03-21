using UnityEngine;
using TrueSync;
using System.Collections;

public class Sustained : TrueSyncBehaviour
{
    [HideInInspector]
    public int damage;
    [Tooltip("How often the sustained weapon does to the target")]
    public float damageFreq;

    private bool isWaiting = false;

    public void OnTriggerStay (Collider other)
    {
        if (!isWaiting)
        {
            if (other.gameObject.tag == "Player")   //Checks if collided with player
            {
                Health hitPlayer = other.gameObject.GetComponent<Health>();     //Reference the players movement script
                if (hitPlayer.owner != owner)   //Checks to see if the player hit is an enemy and not yourself
                {
                    print("sustained id: " + owner.Id);
                    hitPlayer.TakeDamage(damage, this.owner.Id);                    
                    TrueSyncManager.SyncedStartCoroutine(SendDamage());
                }
            }
        }
    }

    IEnumerator SendDamage()
    {
        isWaiting = true;
        yield return 1;
        isWaiting = false;
    }

    public void OnTriggerExit(Collider other)
    {
        isWaiting = false;
    }
}