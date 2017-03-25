using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class Shooting : NetworkBehaviour
{
    public byte owner;
    
    public float reloadTime = 2f;

    //Sustained Based Vars
    public int overheatMax;     //Max amount of heat that can be had
    public float heatUpAmount = 1;  //How fast your weapon gains heat
    public float cooldownHeatDownAmt = 5;    //How fast your weapon loses heat
    public float overheatedHeatDownAmt = 2; //How fast your weapon loses heat when overheated

    //Projectile Based Vars
    [SerializeField]
    int magazineSize = 50;
    [SerializeField]
    int ammo = 0;
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

	void Start() 
	{
        owner = (byte)GetComponent<NetworkIdentity>().netId.Value;
        objectPool = GameObject.Find("PoolManager").GetComponent<NetworkedObjectPooling>();
        CmdInitOnServer();
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
            SetAmmoText();
            sustainedProjectile.SetActive(false);
        }
    }

    [Command]
    void CmdInitOnServer()
    {
        objectPool = GameObject.Find("PoolManager").GetComponent<NetworkedObjectPooling>();
        sustained = sustainedProjectile.GetComponent<Sustained>();
        sustained.damage = damage;
        sustained.owner = owner;
    }

    void Update()
    {
        if (NotSoPausedPauseMenu.isOn)
            return;

        if (Input.GetButton("Fire1"))
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
                        CallReload();
                    }

                    if (fired)         //Check if fire was pressed
                    {
                        if (!isShooting && !isWaiting)
                        {
                            Fire();

                            if (ammo <= 0)   //Check ammo, if zero reload
                                CallReload();
                        }
                    }
                }
                break;
            case CurrentWeapon.Laser: //If weapon is the laser
            case CurrentWeapon.Flamethrower: //If weapon is a flamethrower
                if (fired)  
                {
                    if (!overheated)
                        FireSustained();
                }
                else 
                {
                    if (laserHeat >= 0)
                    {
                        CmdFireSustained(false);
                        StartCoroutine(Cooling());
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

    [Client]
    void CallReload()
    {
        if (!isReloading)
        {
            isReloading = true;
            Invoke("Reload", reloadTime);
        }
    }
    
    void Reload()    //Reload and allow shooting after reloadTime
    {        
        ammo = magazineSize;
        SetAmmoText();
        isReloading = false;
    }

    void Fire()
    {
        isShooting = true;
        muzzleFlash.SetActive(true);
        sfx.PlayProjectileSFX();
        int _damage = (int)(damage * damageMulitplier);
        CmdFireProjectile(gunBarrel.transform.position, gunBarrel.transform.up, owner, projectileSpeed, _damage);
        StartCoroutine(Wait(_fireFreq));
        muzzleFlash.SetActive(false);
        isShooting = false;
        ammo -= 1;              //Subtract ammo
        SetAmmoText();
    }

    [Command]
    void CmdFireProjectile( Vector3 position, Vector3 direction, byte anOwner, float speed, int damage)
    {
        Debug.LogError(objectPool);
        var obj = objectPool.GetFromPool(position);                             //Grab bullet from pool

        Projectile projectile = obj.GetComponent<Projectile>();                 //Set the projectile script
        projectile.owner = anOwner;                                             //Assigning the owner
        projectile.damage = damage;                                             //assigning the damage
        obj.GetComponent<Rigidbody>().velocity = direction * speed;

        NetworkServer.Spawn(obj);                                               // spawn bullet on client, custom spawn handler will be called      , objectPool.assetId
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

    void FireSustained()
    {
        sustained.damage = (int)(damage * damageMulitplier);
        CmdFireSustained(true);

        if (!sustainedProjectile.activeInHierarchy)
            sfx.PlaySustainedSFX(currentWeapon.ToString());

        laserHeat = laserHeat + heatUpAmount;

        if (laserHeat >= overheatMax)
            StartCoroutine(Overheated());
    }

    [Command]
    void CmdFireSustained(bool showSustained)
    {
        RpcShowSustained(showSustained);                
    }

    [ClientRpc]
    void RpcShowSustained(bool isActive)
    {
        sustainedProjectile.SetActive(isActive);
    }

    IEnumerator Overheated()
    {
        overheated = true;
        CmdFireSustained(false);
        sfx.StopSustainedSFX();

        for (float i = laserHeat; i > 0; i--)
        {
            if (!isHoldingTrigger)
            {
                laserHeat = laserHeat - overheatedHeatDownAmt;
                yield return new WaitForSeconds(timeConverter);
                if (laserHeat <= 0)
                {
                    overheated = false;
                }
            }
            else //traps the IEnumerator until the trigger is let gone
            {
                i++;
                yield return new WaitForSeconds(0);
            }
        }
    }

    IEnumerator Cooling()
    {
        if (!cooling && !overheated)
        {
            cooling = true;
            laserHeat = laserHeat - cooldownHeatDownAmt;
            yield return new WaitForSeconds(timeConverter);
            cooling = false;
        }
    }

    [Client]
    void SetAmmoText()
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
