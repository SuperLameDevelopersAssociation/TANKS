using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using TrueSync;

public class Shooting : TrueSyncBehaviour
{
    public FP magazineSize;
    [AddTracking]
    public FP reloadTime;

    //Sustained Based Vars
    public int overheatMax;     //Max amount of heat that can be had
    public float heatUpAmount = 1;  //How fast your weapon gains heat
    public float cooldownHeatDownAmt = 5;    //How fast your weapon loses heat
    public float overheatedHeatDownAmt = 2; //How fast your weapon loses heat when overheated

    //Projectile Based Vars
    [AddTracking]
    private FP ammo = 0;
    [AddTracking]
    private byte isReloading = 0; //if isReloading = 0 then its false, else if its 1 then its true
    [AddTracking]
    private byte isShooting = 0; //if isShooting = 0 then its false, else if its 1 then its true

    //Weapon Variables
    [HideInInspector]
    public GameObject projectileType;
    [HideInInspector]
    public GameObject sustainedProjectile;
    [HideInInspector]
    public int damage;
    [HideInInspector]
    public float projectileSpeed;
    [HideInInspector]
    public float fireFreq;
    [HideInInspector]
    public float cooldown;

    public enum CurrentWeapon {Projectile, Laser, Flamethrower};   //This would be set in the script that instantiates the players. 

    [AddTracking]
    FP laserHeat;   //Current laser heat
	FP _fireFreq;
    bool overheated;    //If the weapon is overheated
    bool cooling;
    bool isHoldingTrigger;  //Fire1 is pressed/

    public CurrentWeapon currentWeapon;


    Text ammoText;

    private GameObject gunBarrel;
    private GameObject turretWrangler;

    public int poolSize = 10;

	void Start() 
	{
        if (transform.FindChild("Canvas").FindChild("Ammo"))
        {
            ammoText = transform.FindChild("Canvas").FindChild("Ammo").GetComponent<Text>();
        }
        else
        {
            Debug.LogError("There is no text object called Ammmo.");
        }

        _fireFreq = fireFreq;
    }

    public override void OnSyncedStart()
    {
        //Instantiate pool
        //parameters are gameobject bullet, int number of pooled objects
        PoolManagerScript.instance.CreatePool(projectileType, poolSize);
        if (currentWeapon.Equals(CurrentWeapon.Flamethrower) || currentWeapon.Equals(CurrentWeapon.Laser))
        {
            Sustained sustained = sustainedProjectile.GetComponent<Sustained>();
            sustained.damage = damage;
        }
        else
        {
            turretWrangler = transform.FindChild("TurretWrangler").gameObject;
            gunBarrel = turretWrangler.transform.FindChild("Turret").transform.FindChild("Barrel").gameObject.transform.FindChild("GunBarrel").gameObject;
            ammo = magazineSize;
            sustainedProjectile.SetActive(false);
        }
    }
    public override void OnSyncedInput()
    {
        if (Input.GetButton("Fire1"))       //Set the inputs for fire1
            TrueSyncInput.SetByte(2, 1);
        else
            TrueSyncInput.SetByte(2, 0);

        if (Input.GetKey(KeyCode.R))
            TrueSyncInput.SetByte(3, 1);
        else
            TrueSyncInput.SetByte(3, 0);
    }

    public override void OnSyncedUpdate()
    {
        byte fire = TrueSyncInput.GetByte(2);   //Get the input
        byte reloadButton = TrueSyncInput.GetByte(3);

        switch (currentWeapon)
        {
            //For right now we only have projectiles and sustained weapons so I could do an if else for this but just in case in the future we come up with some other weapon type that does not fit either of 
            //The ones we have we would have to do a switch so i will do that for now just in case.
            case CurrentWeapon.Projectile: //If weapon is the projectile

                if(reloadButton == 1 && isReloading == 0)
                {
                    TrueSyncManager.SyncedStartCoroutine(Reload());
                }
                if (fire == 1 && isReloading == 0 && isShooting == 0)         //Check if it was pressed
                {
                    isShooting = 1;
                    ammo -= 1;    //Subtract ammo
                    TrueSyncManager.SyncedStartCoroutine(FireProjectile());
                    if (ammo <= 0)   //Check ammo, if zero reload
                        TrueSyncManager.SyncedStartCoroutine(Reload());
                }
                break;
            case CurrentWeapon.Laser: //If weapon is the laser
            case CurrentWeapon.Flamethrower: //If weapon is a flamethrower
                if (fire == 1)
                {
                    if (!overheated)
                        FireSustained();
                }
                else if (fire == 0 && laserHeat >= 0)
                {
                    sustainedProjectile.SetActive(false);
                    TrueSyncManager.SyncedStartCoroutine(Cooling());
                }
                else if (laserHeat < 0)
                    laserHeat = 0;

                if (fire == 1)
                    isHoldingTrigger = true;
                else
                    isHoldingTrigger = false;

                break;
        }
    }

    IEnumerator Reload()    //Reload and allow shooting after reloadTime
    {
        isReloading = 1;
        ammo = magazineSize;
        yield return 3;
        isReloading = 0;
    }

    IEnumerator FireProjectile()
    {//This script was modified by Chris
        GameObject bullet = projectileType;

        bullet.GetComponent<TSTransform>().position = gunBarrel.transform.position.ToTSVector();
        Projectile projectile = bullet.GetComponent<Projectile>();    //Set the projectile script
        projectile.direction = turretWrangler.transform.forward; //Set the projectiles direction
        projectile.actualDirection = projectile.direction.ToTSVector();
        projectile.owner = owner;   //Find the owner
        projectile.speed = projectileSpeed;
        projectile.damage = damage;//assigning the damage
        bullet.SetActive(true);
        TSVector pos = gunBarrel.transform.position.ToTSVector();
        //parameters are gameobject bullet, TSvector position, and TSVector rotation
        PoolManagerScript.instance.ReuseObject(bullet, pos, projectile.actualDirection, TSQuaternion.identity);
        yield return _fireFreq;
        isShooting = 0;
    }

    void FireSustained()
    {
        if(!sustainedProjectile.activeInHierarchy)
        {
            sustainedProjectile.SetActive(true);
        }

        laserHeat = laserHeat + heatUpAmount;
       // print("WeaponActive... Laserheat is at " + laserHeat);

        if(laserHeat >= overheatMax)
        {
            TrueSyncManager.SyncedStartCoroutine(Overheated());
        }
    }
    IEnumerator Overheated()
    {
        overheated = true;
        sustainedProjectile.SetActive(false);
        for (FP i = laserHeat; i > 0; i = i - 1)
        {
            if(!isHoldingTrigger)
            {
              //  print("Not holding trigger");
                laserHeat = laserHeat - overheatedHeatDownAmt;
             //   print("Overheating... LaserHeat is at " + laserHeat);
                yield return 0.1;
                if (laserHeat <= 0)
                {
                    overheated = false;
                }
            }
            else
            {
                i = i + 1;
                yield return 0;
            }
        }
    }
    IEnumerator Cooling()
    {
        if(!cooling && !overheated && laserHeat > 0)
        {
            cooling = true;
            laserHeat = laserHeat - cooldownHeatDownAmt;
         //   print("Weapon Cooling Down.. Laserheat is at " + laserHeat);
            yield return 0.1;
            cooling = false;
        }
    }

    void OnGUI()
    {
        ammoText.text = " " + ammo + " / " + magazineSize;
    }
}
