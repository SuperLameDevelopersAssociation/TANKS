using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class Shooting : NetworkBehaviour
{
    public byte ID;
    
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
    public GameObject laserObject;
    [HideInInspector]
    public GameObject flamethrowerObject;
    [HideInInspector]
    public int projectileDamage;
    [HideInInspector]
    public int flamethrowerDamage;
    [HideInInspector]
    public int laserDamage;
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
    bool damageBoosted;

    public CurrentWeapon currentWeapon;

    Text ammoText;

    Sustained sustained;
    ShootingSFX sfx;

    private GameObject gunBarrel;
    private GameObject turretWrangler;
    private GameObject muzzleFlash;

	void Start() 
	{
        //ID = (byte)GetComponent<NetworkIdentity>().netId.Value;
        objectPool = GameObject.Find("PoolManager").GetComponent<NetworkedObjectPooling>();
        CmdInitOnServer(currentWeapon.ToString());
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

        if (currentWeapon.Equals(CurrentWeapon.Flamethrower))
        {
            sustained = flamethrowerObject.GetComponent<Sustained>();
            sustained.damage = flamethrowerDamage;
            sustained.ID = ID;
            ammoText.gameObject.SetActive(false);
        }
        else if (currentWeapon.Equals(CurrentWeapon.Laser))
        {
            sustained = laserObject.GetComponent<Sustained>();
            sustained.damage = laserDamage;
            sustained.ID = ID;
            ammoText.gameObject.SetActive(false);
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
            flamethrowerObject.SetActive(false);
            laserObject.SetActive(false);
        }
    }
    
    public void ResetAmmo()
    {
        if(currentWeapon.Equals(CurrentWeapon.Projectile))
        {
            ammo = magazineSize;
            SetAmmoText();
        }
        else
        {
            laserHeat = 0;
        }
    }

    [Command]
    void CmdInitOnServer(string currentWeap)
    {
        objectPool = GameObject.Find("PoolManager").GetComponent<NetworkedObjectPooling>();
        if (currentWeap == CurrentWeapon.Flamethrower.ToString())
        {
            sustained = flamethrowerObject.GetComponent<Sustained>();
            sustained.damage = flamethrowerDamage;
            sustained.ID = ID;
        }
        else if (currentWeap == CurrentWeapon.Flamethrower.ToString())
        {
            sustained = laserObject.GetComponent<Sustained>();
            sustained.damage = laserDamage;
            sustained.ID = ID;
        }
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
                        FireSustained(currentWeapon.ToString());
                }
                else 
                {
                    if (laserHeat >= 0)
                    {
                        CmdFireSustained(false, currentWeapon.ToString());
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
        int _damage = (int)(projectileDamage * damageMulitplier);
        CmdFireProjectile(gunBarrel.transform.position, gunBarrel.transform.up, ID, projectileSpeed, _damage);
        StartCoroutine(Wait(_fireFreq));
        muzzleFlash.SetActive(false);
        isShooting = false;
        ammo -= 1;              //Subtract ammo
        SetAmmoText();
    }

    [Command]
    void CmdFireProjectile( Vector3 position, Vector3 direction, byte ownerID, float speed, int damage)
    {
        var obj = objectPool.GetFromPool(position);                             //Grab bullet from pool

        Projectile projectile = obj.GetComponent<Projectile>();                 //Set the projectile script
        projectile.ID = ownerID;                                             //Assigning the owner
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

    void FireSustained(string currentWeap)
    {
        if (currentWeap == CurrentWeapon.Flamethrower.ToString())
        {
            sustained.damage = (int)(flamethrowerDamage * damageMulitplier);
            CmdFireSustained(true, currentWeap);

            if (!flamethrowerObject.activeInHierarchy)
                sfx.PlaySustainedSFX(currentWeapon.ToString());
        }
        else if (currentWeap == CurrentWeapon.Laser.ToString())
        {
            sustained.damage = (int)(laserDamage * damageMulitplier);
            CmdFireSustained(true, currentWeap);

            if (!laserObject.activeInHierarchy)
                sfx.PlaySustainedSFX(currentWeapon.ToString());
        }

        laserHeat = laserHeat + heatUpAmount;

        if (laserHeat >= overheatMax)
            StartCoroutine(Overheated());
    }

    [Command]
    void CmdFireSustained(bool showSustained, string currentWeap)
    {
        RpcShowSustained(showSustained, currentWeap);
    }

    [ClientRpc]
    void RpcShowSustained(bool isActive, string currentWeap)
    {
        if (currentWeap == CurrentWeapon.Flamethrower.ToString())
            flamethrowerObject.SetActive(isActive);
        else if (currentWeap == CurrentWeapon.Laser.ToString())
            laserObject.SetActive(isActive);
    }

    IEnumerator Overheated()
    {
        overheated = true;
        CmdFireSustained(false, currentWeapon.ToString());
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

    [Command]
    public void CmdGiveDamageBoost(float multiplier, int duration)
    {
        RpcGiveDamageBoost(multiplier, duration);
    }

    [ClientRpc]
    public void RpcGiveDamageBoost(float multiplier, int duration)
    {
        StartCoroutine(GiveDamageBoost(multiplier, duration));
    }

    //===============Give Damage Boost (Called By DamageBoost)===========
    public IEnumerator GiveDamageBoost(float multiplier, int duration)
    {
        damageMulitplier = multiplier;
        yield return new WaitForSeconds(duration);
        damageMulitplier = 1;
    }
}
