using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Projectile : NetworkBehaviour
{
    public short owner;
    [HideInInspector]
    public float speed = 15;           //Store speed for projectile
    [HideInInspector]
    public Vector3 direction;       //Store the direction

    [HideInInspector]
    public int damage; 

    //void OnEnable()
    //{
    //    if(Time.timeSinceLevelLoad > 1)
    //        TrueSyncManager.SyncedStartCoroutine(CmdDestroyBullet(3));
    //}

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);   //Move the projectile
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")   //Checks if collided with player
        {
            Health hitPlayer = other.gameObject.GetComponent<Health>();     //Reference the players movement script
            if (hitPlayer.owner != owner)   //Checks to see if the player hit is an enemy and not yourself
            {
                hitPlayer.TakeDamage(damage, owner);
                //StartCoroutine(CmdDestroyBullet(0));
                CmdDestroyBullet(0);
            }
        }

    }

    [Command]
    void CmdDestroyBullet(float waitTime)
    {
        //yield return new WaitForSeconds(waitTime);
        gameObject.SetActive(false);
    }
}