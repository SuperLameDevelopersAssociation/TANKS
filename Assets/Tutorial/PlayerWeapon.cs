using UnityEngine;
using System.Collections;
using TrueSync;

public class PlayerWeapon : TrueSyncBehaviour
{
    public GameObject projectilePrefab;
    public FP magazineSize;
    [AddTracking]
    public FP reloadTime;

    [AddTracking]
    public FP fireFreq;

    [AddTracking]
    private FP ammo = 0;
    [AddTracking]
    private byte isReloading = 0; //if isReloading = 0 then its false, else if its 1 then its true
    [AddTracking]
    private byte isShooting = 0; //if isShooting = 0 then its false, else if its 1 then its true

    public override void OnSyncedStart()
    {
        ammo = magazineSize;
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
        if (fire == 1 && isReloading == 0 && isShooting == 0)         //Check if it was pressed
        {
            isShooting = 1;
            ammo -= 1;    //Subtract ammo
            StartCoroutine(Fire());
            if(ammo <= 0)   //Check ammo, if zero reload
            StartCoroutine(Reload());
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

    IEnumerator Fire()
    {
        //Instantiate bullet
        GameObject projectileObject = TrueSyncManager.SyncedInstantiate(projectilePrefab, tsTransform.position, TSQuaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();    //Set the projectile script
        projectile.direction = tsTransform.forward; //Set the projectiles direction
        projectile.owner = owner;   //Find the owner
        yield return new WaitForSeconds(1);
        isShooting = 0;
    }
}
