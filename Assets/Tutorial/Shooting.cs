using UnityEngine;
using System.Collections;
using TrueSync;

public class Shooting : TrueSyncBehaviour
{
    public FP magazineSize;
    [AddTracking]
    public FP reloadTime;

    [AddTracking]
    private FP ammo = 0;
    [AddTracking]
    private byte isReloading = 0; //if isReloading = 0 then its false, else if its 1 then its true
    [AddTracking]
    private byte isShooting = 0; //if isShooting = 0 then its false, else if its 1 then its true

    Weapons weapons;

    //Weapon Variables
    [HideInInspector]
    public GameObject projectileType;
    [HideInInspector]
    public int damage;
    [HideInInspector]
    public FP projectileSpeed;
    [HideInInspector]
    public float fireFreq;
    [HideInInspector]
    public float cooldown;
    [HideInInspector]
    public float duration;

    [HideInInspector]
    public int currentWeapon;   //This would be set in the script that instantiates the players. 

    public override void OnSyncedStart()
    {
       // currentWeapon = 0;        //You can use this to test
        ammo = magazineSize;
        weapons = GameObject.Find("GameManager").GetComponent<Weapons>();
        weapons.ReturnInfo(currentWeapon, this);

    }
    public override void OnSyncedInput()
    {
        if (Input.GetButton("Fire1"))       //Set the inputs for fire1
            TrueSyncInput.SetByte(2, 1);
        else
            TrueSyncInput.SetByte(2, 0);
    }

    public override void OnSyncedUpdate()
    {
        byte fire = TrueSyncInput.GetByte(2);   //Get the input

        switch(currentWeapon)
        {
            //For right now we only have projectiles and sustained weapons so I could do an if else for this but just in case in the future we come up with some other weapon type that does not fit either of 
            //The ones we have we would have to do a switch so i will do that for now just in case.
            case 0: //If weapon is the projectile
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
                if (fire == 1 && isShooting == 0)
                {
                    isShooting = 1;
                    StartCoroutine(FireSustained());
                }
                break;
            case 2: //If weapon is the flamethrower
                if (fire == 1 && isShooting == 0)
                {
                    isShooting = 1;
                    StartCoroutine(FireSustained());
                }
                break;
        }
    }

    IEnumerator Reload()    //Reload and allow shooting after reloadTime
    {
        print("Reloading");
        isReloading = 1;
        ammo = magazineSize;
        yield return new WaitForSeconds(3);
        isReloading = 0;
        print("done reloading");
    }

    IEnumerator FireProjectile()
    {
        print("FireProjectile()");
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

    IEnumerator FireSustained()
    {
        print("FireSustained()");
        projectileType.GetComponent<Projectile>().damage = damage;
        //Activate Laser or Flamethrower
        projectileType.SetActive(true);                 //The script that spawns the players should instantiate the prefab that has the laser/flamethrower positioned already meaning I just have to turn it on and off.
        yield return new WaitForSeconds(duration);
        projectileType.SetActive(false);

        yield return new WaitForSeconds(cooldown);
        isShooting = 0;
    }
}
