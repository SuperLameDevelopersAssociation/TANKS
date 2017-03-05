using UnityEngine;
using TrueSync;
using UnityEngine.UI;
using System;
using System.Collections;

public class Health : TrueSyncBehaviour
{
    public int maxHealth;
    [AddTracking]
    private int currHealth;

    PointsManager pManager;
    public GameObject[] respawnLocations;                   //Stores references to empty game objects as respawn points
    int newSpawnLocation;

    public bool testRespawn = false;
    public Slider healthBar;

	void Start()
	{
        healthBar.maxValue = maxHealth;
		healthBar.value = maxHealth;
        respawnLocations = GameObject.FindGameObjectsWithTag("Respawn");
	}

    public override void OnSyncedStart()
    {
        currHealth = maxHealth;
        pManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PointsManager>();
    }
    public void TakeDamage(int damage, int playerID)
    {
        currHealth -= damage;
        healthBar.value = currHealth;

        if (currHealth <= 0 || testRespawn)        //Call on Respawn Method
        {
            RespawnTank(playerID);
        }
    }
    public void RespawnTank(int playerID)
    {
        newSpawnLocation = TSRandom.Range(0, respawnLocations.Length);

        /* This section should include the Raycast check and advanced checking */
        tsTransform.position = new TSVector(respawnLocations[newSpawnLocation].transform.position.x, 0, respawnLocations[newSpawnLocation].transform.position.z); //respawn randomly
        tsTransform.rotation = TSQuaternion.identity;
        gameObject.GetComponent<TSRigidBody>().velocity = TSVector.zero;
        int killedId = (this.owner.Id - 1); //both minus one to make it work with indexs
        int killerId = (playerID - 1);
        currHealth = maxHealth;
        healthBar.value = currHealth;
        pManager.AwardPoints(killerId, killedId);
    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 100 + 30 * owner.Id, 300, 30), "player: " + owner.Id + ", health: " + currHealth);
        //GUI.Label(new Rect(10, 140 + 30 * owner.Id, 300, 30), "Deaths: " + deaths + ", Kills: " + PhotonNetwork.playerList[owner.Id].GetScore());
    }
}
