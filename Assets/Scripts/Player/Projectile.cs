﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Projectile : NetworkBehaviour
{
    public byte owner;

    [HideInInspector]
    public int damage;

    NetworkedObjectPooling objectPool;

    void Start()
    {
        objectPool = GameObject.Find("PoolManager").GetComponent<NetworkedObjectPooling>();
    }

    void OnEnable()
    {
        if (Time.timeSinceLevelLoad > 1)
            StartCoroutine(DestroyBullet(3));
    }

    [Server]
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")   //Checks if collided with player
        {
            Health hitPlayer = other.gameObject.GetComponent<Health>();     //Reference the players movement script
            if (hitPlayer.owner != owner)   //Checks to see if the player hit is an enemy and not yourself
            {
                hitPlayer.RpcTakeDamage(damage, owner);
                StartCoroutine(DestroyBullet(0));
            }
        }
    }

    IEnumerator DestroyBullet(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        objectPool.UnSpawnObject(gameObject);
        NetworkServer.UnSpawn(gameObject);
    }
}