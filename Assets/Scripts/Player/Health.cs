﻿using UnityEngine;
using TrueSync;
using System.Collections;

public class Health : TrueSyncBehaviour
{
    public int maxHealth;
    [AddTracking]
    private int currHealth;

    //Weapons weapons

    public override void OnSyncedStart()
    {
        currHealth = maxHealth;
        //Weapons = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Weapons>();
    }


    public void TakeDamage(string tag)
    {
        int damage = 5; //find how much damage the weapons type is from Weapons
        currHealth -= damage;

        if (currHealth <= 0)
        {
            tsTransform.position = new TSVector(TSRandom.Range(-50, 50), 0, TSRandom.Range(-50, 50)); //respawn randomly
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        //send over information about how killed who
        yield return new WaitForSeconds(.1f);
        currHealth = maxHealth;
    }
}
