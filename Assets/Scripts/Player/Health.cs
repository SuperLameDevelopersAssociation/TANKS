using UnityEngine;
using TrueSync;
using UnityEngine.UI;
using System.Collections;

public class Health : TrueSyncBehaviour
{
    public int maxHealth;
    [AddTracking]
    public int currHealth;

    PointsManager pManager;

	public Slider healthBar;

    [Range(1, 5)]
    public int armorLevel;

    float armorBonus;

	void Start()
	{
        healthBar.maxValue = maxHealth;
		healthBar.value = maxHealth;
        SetArmor();
	}

    public override void OnSyncedStart()
    {
        currHealth = maxHealth;
        pManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PointsManager>();
    }

    public void TakeDamage(int damage, int playerID)
    {
        damage -= (int)(damage * armorBonus);               //apply armor bonus
        currHealth -= damage;
		healthBar.value = currHealth;

        if (currHealth <= 0)
        {
            tsTransform.position = new TSVector(TSRandom.Range(-50, 50), 0, TSRandom.Range(-50, 50)); //respawn randomly
            tsTransform.rotation = TSQuaternion.identity;
            gameObject.GetComponent<TSRigidBody>().velocity = TSVector.zero;
            int killedId = (this.owner.Id - 1); //both minus one to make it work with indexs
            int killerId = (playerID - 1);
            currHealth = maxHealth;
            healthBar.value = currHealth;
            pManager.AwardPoints(killerId, killedId);
        }
    }
    
    //Sets the resistance given by armor and lowers speed according to the armor level
    public void SetArmor()
    {
        armorBonus = armorLevel / 10.0f;
        GetComponent<PlayerMovement>().speed -= (int)TSMath.Ceiling(GetComponent<PlayerMovement>().speed * armorBonus);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 100 + 30 * owner.Id, 300, 30), "player: " + owner.Id + ", health: " + currHealth);
        //GUI.Label(new Rect(10, 140 + 30 * owner.Id, 300, 30), "Deaths: " + deaths + ", Kills: " + PhotonNetwork.playerList[owner.Id].GetScore());
    }
}
