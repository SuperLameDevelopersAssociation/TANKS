using UnityEngine;
using System.Collections;
using TrueSync;

public class Shooting : TrueSyncBehaviour
{
    public FP magazineSize;
    [AddTracking]
    public FP reloadTime;

    public int overheatMax;     //Max amount of heat that can be had
    public float heatUpAmount = 1;  //How fast your weapon gains heat
    public float cooldownHeatDownAmt = 5;    //How fast your weapon loses heat
    public float overheatedHeatDownAmt = 2; //How fast your weapon loses heat when overheated

    [AddTracking]
    private FP ammo = 0;
    [AddTracking]
    private byte isReloading = 0; //if isReloading = 0 then its false, else if its 1 then its true
    [AddTracking]
    private byte isShooting = 0; //if isShooting = 0 then its false, else if its 1 then its true

    //Weapon Variables
    public GameObject projectileType;
    public GameObject sustainedProjectile;
    public int damage;
    public FP projectileSpeed;
    public float fireFreq;
    public float cooldown;
    public float duration;

    [HideInInspector]
    public int currentWeapon;   //This would be set in the script that instantiates the players. 

    [AddTracking]
    FP laserHeat;   //Current laser heat
    bool overheated;    //If the weapon is overheated
    bool cooling;
    bool isHoldingTrigger;  //Fire1 is pressed/

    public override void OnSyncedStart()
    {
        currentWeapon = 1;        //You can use this to test

        if (currentWeapon == 1 || currentWeapon == 2)
        {
            Sustained sustained = sustainedProjectile.GetComponent<Sustained>();
            sustained.damage = damage;
        }
        else
        {
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
            case 0: //If weapon is the projectile

                if(reloadButton == 1 && isReloading == 0)
                {
                    StartCoroutine(Reload());
                }
                if (fire == 1 && isReloading == 0 && isShooting == 0)         //Check if it was pressed
                {
                    isShooting = 1;
                    ammo -= 1;    //Subtract ammo
                    StartCoroutine(FireProjectile());
                    if (ammo <= 0)   //Check ammo, if zero reload
                        StartCoroutine(Reload());
                }
                break;
            case 1: //If weapon is the laser
            case 2: //If weapon is a flamethrower
                if (fire == 1)
                {
                    if (!overheated)
                        FireSustained();
                }
                else if (fire == 0 && laserHeat >= 0)
                {
                    sustainedProjectile.SetActive(false);
                    StartCoroutine(Cooling());
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
     //   print("Reloading");
        isReloading = 1;
        ammo = magazineSize;
        yield return new WaitForSeconds(3);
        isReloading = 0;
      //  print("done reloading");
    }

    IEnumerator FireProjectile()
    {
        //print("FireProjectile()");
        //Instantiate bullet
        GameObject projectileObject = TrueSyncManager.SyncedInstantiate(projectileType, tsTransform.position, TSQuaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();    //Set the projectile script
        projectile.direction = tsTransform.forward; //Set the projectiles direction
        projectile.owner = owner;   //Find the owner
        projectile.speed = projectileSpeed;
        projectile.damage = damage;
        yield return new WaitForSeconds(fireFreq);
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
            StartCoroutine(Overheated());
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
                yield return new WaitForSeconds(.1f);
                if (laserHeat <= 0)
                {
                    overheated = false;
                }
            }
            else
            {
                i = i + 1;
                yield return new WaitForSeconds(0);
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
            yield return new WaitForSeconds(.1f);
            cooling = false;
        }
    }
}
