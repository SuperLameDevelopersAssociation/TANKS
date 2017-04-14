using UnityEngine;
using UnityEngine.Networking;
//using System.Collections;

public class PlayerSetup : NetworkBehaviour 
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    [SerializeField]
    GameObject[] weaponObjects;
    [SerializeField]
    GameObject turretWrangler;
    [SyncVar]
    public byte ID;
    [HideInInspector]
    public Transform spawnPoint;
    [SyncVar]
    public int tankSelected;
    [SerializeField]
    NetworkAnimator anim;

    void Start()
    {
        if (!isLocalPlayer)
        {
            gameObject.name = "RemotePlayer";
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
        else
        {
            gameObject.name = "LocalPlayer";            
        }

        GetComponent<Health>().ID = ID;
        GetComponent<Shooting>().ID = ID;
        switch(tankSelected)
        {
            case 0:
                GetComponent<Shooting>().currentWeapon = Shooting.CurrentWeapon.Projectile;
                break;
            case 1:
                GetComponent<Shooting>().currentWeapon = Shooting.CurrentWeapon.Laser;
                break;
            case 2:
                GetComponent<Shooting>().currentWeapon = Shooting.CurrentWeapon.Flamethrower;
                break;
            default:
                break;
        }

        for (int i = 0; i < weaponObjects.Length; i++)
        {
            if (i == tankSelected)
            {
                weaponObjects[i].SetActive(true);
                anim.animator = weaponObjects[i].GetComponent<Animator>();
                turretWrangler.GetComponent<NetworkedMouseLook>().animator = anim.animator;
            }
            else
                weaponObjects[i].SetActive(false);
        }
/*
        Debug.LogError(transform.position);

        if (transform.position == Vector3.zero)
        {
            Debug.LogError("Reset the point");

            Transform _spawnPoint = GameManager.instance.spawnPoints[3].transform;
            transform.position = _spawnPoint.position;
            transform.rotation = _spawnPoint.rotation;
        }
*/
    }
}
