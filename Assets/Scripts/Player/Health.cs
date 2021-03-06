﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class Health : NetworkBehaviour
{
    public byte ID;
    public int maxHealth;
    [SyncVar]
    public int currHealth;

    [Header("Health UI")]
    public int yellowColorHealth = 68;
    public int redColorHealth = 17;

    public Text healthPercent;
    public Image healthImage;

    private int defenseDamage = 0;
    private int originalMaxHealth;
    private bool defenseBoost = false;

    [SyncVar]
    public bool inSpawn = true;

    [Range(1, 5)]
    public int armorLevel;

    float armorBonus;
    bool respawning;

    DamageSFX damageSound;
    Shooting currentWeapon;
    PowerUpVisual powerVisual;

    void Awake()
	{        
        originalMaxHealth = maxHealth;
        Invoke("SetArmor", 1);
        currHealth = maxHealth;
        SetHealthUI();
        Invoke("CmdSetHealth", 1);

        damageSound = gameObject.GetComponent<DamageSFX>();

        if (damageSound == null)
        { 
            Debug.LogError("There is no DamageSFX script attached to " + gameObject.name);
        }

        currentWeapon = gameObject.GetComponent<Shooting>();
        powerVisual = GetComponent<PowerUpVisual>();

    }

    [Command]
    void CmdSetHealth()
    {
        originalMaxHealth = maxHealth;
        SetArmor();
        currHealth = maxHealth;
    }

    [Command]
    public void CmdTakeDamage(int damage, byte murdererID)
    {
        RpcTakeDamage(damage, murdererID);
    }

    [ClientRpc]
    public void RpcTakeDamage(int damage, byte murdererID)
    {
        if (inSpawn) return;

        damage -= (int)(damage * armorBonus);               //apply armor bonus
        currHealth -= damage;
        SetHealthUI();

        damageSound.PlayDamageSFX(currentWeapon.currentWeapon.ToString());

        if (defenseBoost)
        {                                 // Check if the defense boost is depleted.
            defenseDamage += damage;
            if (defenseDamage >= 100)
            {
                EndDefenseBoost();
            }
        }

        if (currHealth <= 0 && !respawning)
        {
            respawning = true;
            StartCoroutine(Respawn());
            GameManager.GetInstance.AwardPoints(murdererID, ID);
        }
    }

    [ClientRpc]
    public void RpcHasDied()
    {
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.GetInstance.respawnTime);
        Transform _spawnPoint = GameManager.GetInstance.spawnPoints[ID].transform;
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        currHealth = maxHealth;

        if (isLocalPlayer)
            currentWeapon.ResetAmmo();

        SetHealthUI();
        respawning = false;
    }

    //---translates the amount of health into a color--
    public Color32 GetHealthColor()
    {
        float percent = ((float)currHealth / maxHealth);

        percent = (percent * 100);

        if (percent < redColorHealth)
            return Color.red;
        else if (percent < yellowColorHealth)
            return Color.yellow;
        else
            return Color.green;
    }

    public void GetTextPercentHealth(Text healthText)
    {
        healthText.text = "" + currHealth + " / " + maxHealth;
    }

    //Sets the resistance given by armor and lowers speed according to the armor level
    public void SetArmor()
    {
        armorBonus = armorLevel / 10.0f;
        GetComponent<PlayerMovement>().SetArmorSpeed(armorBonus); //Ceiling(GetComponent<PlayerMovement>().speed * armorBonus);
    }

    public bool IsHealthFull()
    {
        return currHealth == maxHealth;
    }

    [ClientRpc]
    public void RpcAddHealth(int extraHealth)
    {
        currHealth += extraHealth;

        if (currHealth > maxHealth)
        {
            currHealth = maxHealth;
        }

        SetHealthUI();

        if (isLocalPlayer)
            StartCoroutine(HealthBubble());
        
    }

    IEnumerator HealthBubble()
    {
        powerVisual.CmdSetBubble(true, "Health");
        yield return new WaitForSeconds(1.5f);
        powerVisual.CmdSetBubble(false, "Health");
    }

    [Client]
    public void SetHealthUI()
    {
        GetTextPercentHealth(healthPercent);
        healthImage.color = GetHealthColor();
    }

    [ClientRpc]
    public void RpcDefenseBoost(int _maxHealth)
    {
        if (maxHealth <= originalMaxHealth)
        {
            //start shield prefab
            maxHealth += _maxHealth;
            currHealth += _maxHealth;
            SetHealthUI();
            defenseBoost = true;

            if (isLocalPlayer)
                powerVisual.CmdSetBubble(true, "Shield");
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

        //turn off shield prefab 
        if (isLocalPlayer)
            powerVisual.CmdSetBubble(false, "Shield");

        SetHealthUI();
    }
}
