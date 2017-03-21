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
    private bool isReloading = false; //if isReloading = 0 then its false, else if its 1 then its true
    [AddTracking]
    private bool isShooting = false; //if isShooting = 0 then its false, else if its 1 then its true

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
    public double damageMulitplier = 1;

    ObjectPooling objectPool;

    public enum CurrentWeapon {Projectile, Laser, Flamethrower};   //This would be set in the script that instantiates the players. 

    [AddTracking]
    FP laserHeat;   //Current laser heat
	FP _fireFreq;
    FP timeConverter = .1; //makes coortinues run every 1/10 of second
    bool overheated;    //If the weapon is overheated
    bool cooling;
    bool isHoldingTrigger;  //Fire1 is pressed/

    public CurrentWeapon currentWeapon;

    Text ammoText;

    Sustained sustained;
    ShootingSFX sfx;

    private GameObject gunBarrel;
    private GameObject turretWrangler;

	void Start() 
	{
        objectPool = GameObject.Find("PoolManager").GetComponent<ObjectPooling>();
        //sfx = gameObject.GetComponent<ShootingSFX>();

        if (sfx == null)
            Debug.LogError("There is no ShootingSFX attached to " + gameObject.name);


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
        if (currentWeapon.Equals(CurrentWeapon.Flamethrower) || currentWeapon.Equals(CurrentWeapon.Laser))
        {
            sustained = sustainedProjectile.GetComponent<Sustained>();
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

                if (!isReloading)
                {
                    if (reloadButton == 1) //check if reload is pressed
                    {
                        TrueSyncManager.SyncedStartCoroutine(Reload());
                    }

                    if (fire == 1)         //Check if fire was pressed
                    {
                        if (!isShooting)
                        {
                            isShooting = true;
                            ammo -= 1;    //Subtract ammo
                            TrueSyncManager.SyncedStartCoroutine(FireProjectile());
                            if (ammo <= 0)   //Check ammo, if zero reload
                                TrueSyncManager.SyncedStartCoroutine(Reload());
                        }
                    }
                }
                break;
            case CurrentWeapon.Laser: //If weapon is the laser
            case CurrentWeapon.Flamethrower: //If weapon is a flamethrower
                if (fire == 1)  
                {
                    if (!overheated)
                        FireSustained();
                }
                else 
                {
                    if (laserHeat >= 0)
                    {
                        sustainedProjectile.SetActive(false);
                        TrueSyncManager.SyncedStartCoroutine(Cooling());
                        sfx.StopSustainedSFX();
                    }
                }

                if (laserHeat < 0) //defensive code to check if laserHeat is ever negative
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
        isReloading = true;
        ammo = magazineSize;
        yield return reloadTime;
        isReloading = false;
    }

    IEnumerator FireProjectile()
    { 
        GameObject obj = objectPool.GetPooledObject();

        if (obj == null)
        {
            yield break;
        }

        obj.transform.position = gunBarrel.transform.position;
        obj.transform.rotation = transform.rotation;

        Projectile projectile = obj.GetComponent<Projectile>();    //Set the projectile script
        projectile.direction = turretWrangler.transform.forward; //Set the projectiles direction
        projectile.actualDirection = projectile.direction.ToTSVector();
        projectile.owner = owner;   //Find the owner
        projectile.speed = projectileSpeed;
        projectile.damage = (int) (damage * damageMulitplier);//assigning the damage

        obj.SetActive(true);

        sfx.PlayProjectileSFX();

        yield return _fireFreq;
        isShooting = false;
    }

    void FireSustained()
    {
        if(!sustainedProjectile.activeInHierarchy)
        {
            sustainedProjectile.SetActive(true);
            sfx.PlaySustainedSFX(currentWeapon.ToString());
        }

        sustained.damage = (int) (damage * damageMulitplier);
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
        sfx.StopSustainedSFX();

        for (FP i = laserHeat; i > 0; i = i - 1)
        {
            if(!isHoldingTrigger)
            {
              //  print("Not holding trigger");
                laserHeat = laserHeat - overheatedHeatDownAmt;
                //   print("Overheating... LaserHeat is at " + laserHeat);
                yield return timeConverter;
                if (laserHeat <= 0)
                {
                    overheated = false;
                }
            }
            else //traps the IEnumerator until the trigger is let gone
            {
                i = i + 1;
                yield return 0;
            }
        }
    }
    IEnumerator Cooling()
    {
        if(!cooling && !overheated)
        {
            cooling = true;
            laserHeat = laserHeat - cooldownHeatDownAmt;
            //   print("Weapon Cooling Down.. Laserheat is at " + laserHeat);

            yield return timeConverter;
            cooling = false;
        }
    }

    void OnGUI()
    {
        ammoText.text = " " + ammo + " / " + magazineSize;
    }

    //===============Give Damage Boost (Called By DamageBoost)===========
    public IEnumerator GiveDamageBoost(double multiplier,  int duration)
    {
        damageMulitplier = multiplier;
        print("the multiplier is now: " + damageMulitplier);
        yield return duration;
        damageMulitplier = 1;
    }
}
