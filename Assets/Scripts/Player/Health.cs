using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class Health : NetworkBehaviour
{
    public byte owner;
    public int maxHealth;
    [SyncVar]
    public int currHealth;
    private int defenseDamage = 0;
    private int originalMaxHealth;
    private bool defenseBoost = false;

    PointsManager pManager;
    SpawnManager sManager;

	public Slider healthBar;

    [Range(1, 5)]
    public int armorLevel;

    float armorBonus;

	void Start()
	{
        healthBar.maxValue = maxHealth;
		healthBar.value = maxHealth;
        originalMaxHealth = maxHealth;
        SetArmor();
        currHealth = maxHealth;
		SetHealthBar();
        owner = (byte)GetComponent<NetworkIdentity>().netId.Value;
        //pManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PointsManager>();
        //sManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SpawnManager>();
    }

    public void TakeDamage(int damage, int playerID)
    {
        damage -= (int)(damage * armorBonus);               //apply armor bonus
        currHealth -= damage;
		healthBar.value = currHealth;

        if (defenseBoost)
        {                                 // Check if the defense boost is depleted.
            defenseDamage += damage;
            if (defenseDamage >= 100)
            {
                EndDefenseBoost();
            }
        }

        if (currHealth <= 0)
        {
            //sManager.Respawn(owner.Id);
            //int killedId = (this.owner - 1); //both minus one to make it work with indexs
            //int killerId = (playerID - 1);
            currHealth = maxHealth;
            healthBar.value = currHealth;
            //pManager.AwardPoints(killerId, killedId);
        }
    }
    
    //Sets the resistance given by armor and lowers speed according to the armor level
    public void SetArmor()
    {
        armorBonus = armorLevel / 10.0f;
        GetComponent<PlayerMovement>().speed -= Mathf.CeilToInt(GetComponent<PlayerMovement>().speed * armorBonus);//Ceiling(GetComponent<PlayerMovement>().speed * armorBonus);
    }

    public bool isHealthFull()
    {
        return currHealth == maxHealth;
    }

    public void AddHealth(int extraHealth)
    {
        currHealth += extraHealth;

        if (currHealth > maxHealth)
        {
            currHealth = maxHealth;
        }

        SetHealthBar();
    }

	public void SetHealthBar()
	{
		healthBar.value = currHealth;
	}

    //void OnGUI()
    //{
    //    GUI.Label(new Rect(10, 100 + 30 * owner.Id, 300, 30), "player: " + owner.Id + ", health: " + currHealth);
    //}

    public void DefenseBoost(int _maxHealth)
    {
        if (maxHealth <= originalMaxHealth)
        {
            maxHealth += _maxHealth;
            currHealth += _maxHealth;
            defenseBoost = true;
        }
    }

    public void EndDefenseBoost()
    {
        defenseBoost = false;
        defenseDamage = 0;
        if (currHealth > originalMaxHealth)
        {
            currHealth = originalMaxHealth;
        }
        maxHealth = originalMaxHealth;
    }
}
