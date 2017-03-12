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
    public float distanceFromNearestTankToSpawn;

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

        if (currHealth <= 0)        //Call on Respawn Method
        {
            RespawnTank(playerID);
        }
    }
    public void RespawnTank(int playerID)
    {
        tsTransform.position = ChooseRespawnPoint();
        tsTransform.rotation = TSQuaternion.identity;
        gameObject.GetComponent<TSRigidBody>().velocity = TSVector.zero;
        int killedId = (this.owner.Id - 1); //both minus one to make it work with indexs
        int killerId = (playerID - 1);
        currHealth = maxHealth;
        healthBar.value = currHealth;
        pManager.AwardPoints(killerId, killedId);
    }
    TSVector ChooseRespawnPoint()
    {
        TSVector temp;
        /* This section should include the Raycast check and advanced checking */
        GameObject[] playerTanks = GameObject.FindGameObjectsWithTag("Player");
        FP[] dists = new FP[playerTanks.Length];                                          //Check distance between objects

        newSpawnLocation = TSRandom.Range(0, respawnLocations.Length);

        
        for (int i = 0; i < respawnLocations.Length; i++)
        {
            //dists[0] = Vector3.Distance(playerTanks[0].gameObject.transform.position, respawnLocations[i].gameObject.transform.position);
            dists[0] = TSVector.Distance(playerTanks[0].GetComponent<TSTransform>().position, respawnLocations[i].GetComponent<TSTransform>().position);
            //dists[1] = Vector3.Distance(playerTanks[1].gameObject.transform.position, respawnLocations[i].gameObject.transform.position);
            dists[1] = TSVector.Distance(playerTanks[0].GetComponent<TSTransform>().position, gameObject.GetComponent<TSTransform>().position);
            //dists[2] = Vector3.Distance(playerTanks[2].gameObject.transform.position, respawnLocations[i].gameObject.transform.position);
            dists[2] = TSVector.Distance(playerTanks[0].GetComponent<TSTransform>().position, respawnLocations[i].GetComponent<TSTransform>().position);
            //dists[3] = Vector3.Distance(playerTanks[3].gameObject.transform.position, respawnLocations[i].gameObject.transform.position);
            dists[3] = TSVector.Distance(playerTanks[0].GetComponent<TSTransform>().position, respawnLocations[i].GetComponent<TSTransform>().position);

            for (int j= 0; j < dists.Length; j++)
            {
                if(dists[j] < distanceFromNearestTankToSpawn)
                {
                    temp = respawnLocations[i].GetComponent<TSTransform>().position;
                    print("Respawn location is: " + respawnLocations[i].GetComponent<TSTransform>().position);
                    return temp;
                }
                else
                {
                    temp = respawnLocations[TSRandom.Range(0, respawnLocations.Length)].GetComponent<TSTransform>().position;
                }
            }
        }
        return respawnLocations[TSRandom.Range(0, respawnLocations.Length)].GetComponent<TSTransform>().position; ;
    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 100 + 30 * owner.Id, 300, 30), "player: " + owner.Id + ", health: " + currHealth);
        //GUI.Label(new Rect(10, 140 + 30 * owner.Id, 300, 30), "Deaths: " + deaths + ", Kills: " + PhotonNetwork.playerList[owner.Id].GetScore());
    }
}
