using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class Shooting : NetworkBehaviour
{
    public byte owner;
    public int magazineSize;
    public float reloadTime;

    //Sustained Based Vars
    public int overheatMax;     //Max amount of heat that can be had
    public float heatUpAmount = 1;  //How fast your weapon gains heat
    public float cooldownHeatDownAmt = 5;    //How fast your weapon loses heat
    public float overheatedHeatDownAmt = 2; //How fast your weapon loses heat when overheated

    //Projectile Based Vars
    [SyncVar]
    private int ammo = 0;
    [SyncVar]
    private bool isReloading = false;
    [SyncVar]
    private bool isShooting = false;
    bool fired = false;
    bool reloaded = false;

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
    [HideInInspector]
    public double damageMulitplier = 1;

    NetworkedObjectPooling objectPool;

    public enum CurrentWeapon {Projectile, Laser, Flamethrower};   //This would be set in the script that instantiates the players. 

    [SyncVar]
    float laserHeat;                            //Current laser heat
	float _fireFreq;
    float timeConverter = 0.1f;                 //makes coroutinues run every 1/10 of second
    bool overheated;                            //If the weapon is overheated
    bool cooling;
    bool isHoldingTrigger;                      //Fire1 is pressed
    bool isWaiting;

    public CurrentWeapon currentWeapon;

    Text ammoText;

    Sustained sustained;
    ShootingSFX sfx;

    private GameObject gunBarrel;
    private GameObject turretWrangler;
    private GameObject muzzleFlash;

    public int poolSize = 10;

    [Client]
	void Start() 
	{
        owner = (byte)GetComponent<NetworkIdentity>().netId.Value;
        objectPool = GameObject.Find("PoolManager").GetComponent<NetworkedObjectPooling>();
        sfx = gameObject.GetComponent<ShootingSFX>();

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

        if (currentWeapon.Equals(CurrentWeapon.Flamethrower) || currentWeapon.Equals(CurrentWeapon.Laser))
        {
            sustained = sustainedProjectile.GetComponent<Sustained>();
            sustained.damage = damage;
            sustained.owner = owner;
        }
        else
        {
            turretWrangler = transform.FindChild("TurretWrangler").gameObject;
            gunBarrel = turretWrangler.transform.FindChild("Projectile Control").FindChild("Box019").transform.FindChild("GunBarrel").gameObject;
            muzzleFlash = turretWrangler.transform.FindChild("Projectile Control").transform.FindChild("Box019").FindChild("Gun 1 Projectile").FindChild("Muzzle Flash").gameObject;

            if (muzzleFlash == null)
                Debug.LogError("There is no muzzleFlash on " + gameObject.name);
            else
                muzzleFlash.SetActive(false);

            ammo = magazineSize;
            ammoText.text = ammo.ToString();
            sustainedProjectile.SetActive(false);
        }
    }

    void Update()
    {
        if (NotSoPausedPauseMenu.isOn)
            return;

        if (Input.GetButtonDown("Fire1"))
            fired = true;
        else
            fired = false;

        if (Input.GetKeyDown(KeyCode.R))
            reloaded = true;
        else
            reloaded = false;

        switch (currentWeapon)
        {
            case CurrentWeapon.Projectile: //If weapon is the projectile

                if (!isReloading)
                {
                    if (reloaded) //check if reload is pressed
                    {
                        StartCoroutine(Reload());
                    }

                    if (fired)         //Check if fire was pressed
                    {
                        if (!isShooting && !isWaiting)
                        {
                            isShooting = true;
                            ammo -= 1;    //Subtract ammo
                            CmdFireProjectile();

                            if (ammo <= 0)   //Check ammo, if zero reload
                                StartCoroutine(Reload());
                        }
                    }
                }
                break;
            case CurrentWeapon.Laser: //If weapon is the laser
            case CurrentWeapon.Flamethrower: //If weapon is a flamethrower
                if (fired)  
                {
                    if (!overheated)
                        CmdFireSustained();
                }
                else 
                {
                    if (laserHeat >= 0)
                    {
                        sustainedProjectile.SetActive(false);
                        //StartCoroutine(CmdCooling());
                        sfx.StopSustainedSFX();
                    }
                }

                if (laserHeat < 0) //defensive code to check if laserHeat is ever negative
                    laserHeat = 0;

                if (fired) 
                    isHoldingTrigger = true;
                else
                    isHoldingTrigger = false;

                break;
        }
    }

    [Server]
    IEnumerator Reload()    //Reload and allow shooting after reloadTime
    {
        isReloading = true;
        ammo = magazineSize;
        yield return reloadTime;
        isReloading = false;
    }

    [Command]
    void CmdFireProjectile()
    {
        // Set up bullet on server
        GameObject obj = objectPool.GetFromPool(gunBarrel.transform.position, transform.rotation);

        if (obj == null)
        {
            return;
        }

        Projectile projectile = obj.GetComponent<Projectile>();     //Set the projectile script
        projectile.direction = gunBarrel.transform.up;              //Set the projectiles direction
        projectile.owner = owner;                                   //Assigning the owner
        projectile.speed = projectileSpeed;
        projectile.damage = (int)(damage * damageMulitplier);       //assigning the damage
        obj.GetComponent<Rigidbody>().velocity = projectile.direction * projectileSpeed;

        muzzleFlash.SetActive(true);

        sfx.PlayProjectileSFX();

        NetworkServer.Spawn(obj, objectPool.assetId);               // spawn bullet on client, custom spawn handler will be called     

        StartCoroutine(Wait(_fireFreq));

        muzzleFlash.SetActive(false);
        isShooting = false;
    }

    IEnumerator Wait(float waitTime)
    {
        if (!isWaiting)
        {
            isWaiting = true;
            yield return new WaitForSeconds(waitTime);
            isWaiting = false;
        }
    }

    [Command]
    void CmdFireSustained()
    {
        if(!sustainedProjectile.activeInHierarchy)
        {
            sustainedProjectile.SetActive(true);
            sfx.PlaySustainedSFX(currentWeapon.ToString());
        }

        sustained.damage = (int) (damage * damageMulitplier);
        laserHeat = laserHeat + heatUpAmount;

        if(laserHeat >= overheatMax)
        {
            //StartCoroutine(CmdOverheated());
        }
    }

    //[Command]
    //IEnumerator CmdOverheated()
    //{
    //    overheated = true;
    //    sustainedProjectile.SetActive(false);
    //    sfx.StopSustainedSFX();

    //    for (float i = laserHeat; i > 0; i = i - 1)
    //    {
    //        if(!isHoldingTrigger)
    //        {
    //            laserHeat = laserHeat - overheatedHeatDownAmt;
    //            yield return new WaitForSeconds(timeConverter);
    //            if (laserHeat <= 0)
    //            {
    //                overheated = false;
    //            }
    //        }
    //        else //traps the IEnumerator until the trigger is let gone
    //        {
    //            i = i + 1;
    //            yield return new WaitForSeconds(0);
    //        }
    //    }
    //}

    //[Command]
    //IEnumerator CmdCooling()
    //{
    //    if(!cooling && !overheated)
    //    {
    //        cooling = true;
    //        laserHeat = laserHeat - cooldownHeatDownAmt;
    //        yield return new WaitForSeconds(timeConverter);
    //        cooling = false;
    //    }
    //}

    void OnGUI()
    {
        ammoText.text = " " + ammo + " / " + magazineSize;
    }

    //===============Give Damage Boost (Called By DamageBoost)===========
    //[Command]
    //public IEnumerator CmdGiveDamageBoost(double multiplier,  int duration)
    //{
    //    damageMulitplier = multiplier;
    //    yield return new WaitForSeconds(duration);
    //    damageMulitplier = 1;
    //}
}
