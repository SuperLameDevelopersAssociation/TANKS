﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Sustained : NetworkBehaviour
{
    public byte ID;
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
                if (hitPlayer.ID != ID)   //Checks to see if the player hit is an enemy and not yourself
                {
                    StartCoroutine(SendDamage(hitPlayer));
                }
            }
        }
    }

    IEnumerator SendDamage(Health hitPlayer)
    {
        isWaiting = true;
        yield return new WaitForSeconds(damageFreq);
        hitPlayer.RpcTakeDamage(damage, ID);
        isWaiting = false;
    }

    public void OnTriggerExit(Collider other)
    {
        isWaiting = false;
    }
}