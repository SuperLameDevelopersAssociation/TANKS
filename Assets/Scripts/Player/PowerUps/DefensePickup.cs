using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DefensePickup : NetworkBehaviour {

    public int defenseMaxHealth = 100;
    public float destroyTime;

    Health playerHealth;

    void OnEnable()
    {
        if (Time.timeSinceLevelLoad > 1)
            StartCoroutine(Destroy(destroyTime));
    }

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

    IEnumerator Destroy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        NetworkServer.UnSpawn(gameObject);
        Destroy(gameObject);
    }
}
