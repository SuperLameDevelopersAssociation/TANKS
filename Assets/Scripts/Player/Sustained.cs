using UnityEngine;
using TrueSync;
using System.Collections;

public class Sustained : TrueSyncBehaviour
{
    public int damage; //I am not using this variable yet but I put this here to make sure my Shooting script has this set up so when it is implemented it works.

    private bool isWaiting = false;


    public void OnTriggerStay (Collider other)
    {
        if (!isWaiting)
        {
            print(other.gameObject);
            if (other.gameObject.tag == "Player")   //Checks if collided with player
            {
                Health hitPlayer = other.gameObject.GetComponent<Health>();     //Reference the players movement script
                if (hitPlayer.owner != owner)   //Checks to see if the player hit is an enemy and not yourself
                {
                    hitPlayer.TakeDamage(damage);
                    StartCoroutine(SendDamage());
                }
            }
        }
    }

    IEnumerator SendDamage()
    {
        print("SEND DAMAGE");
        isWaiting = true;
        yield return new WaitForSeconds(1f);
        isWaiting = false;
    }


    
}