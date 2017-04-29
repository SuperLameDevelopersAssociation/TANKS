using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PlayerSetup : NetworkBehaviour 
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    [SerializeField]
    GameObject[] weaponObjects;
    [SerializeField]
    GameObject[] tankModels;
    [SerializeField]
    GameObject turretWrangler;
    [SerializeField]
    Sprite[] healthImages;
    [SerializeField]
    Behaviour[] abilties;
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

            foreach (Behaviour ability in abilties)
            {
                ability.enabled = false;
            }
        }
        else
        {
            gameObject.name = "LocalPlayer";
        }

        GetComponent<Health>().ID = ID;
        GetComponent<Shooting>().ID = ID;
        GetComponent<PlayerMovement>().wheels = tankModels[tankSelected].GetComponent<Animator>();

        switch (tankSelected)
        {
            case 0:
                GetComponent<Shooting>().currentWeapon = Shooting.CurrentWeapon.Projectile;
                GetComponent<Health>().healthImage.sprite = healthImages[0];
                abilties[0].enabled = true;
                GetComponent<Health>().armorLevel = 1;
                break;
            case 1:
                GetComponent<Shooting>().currentWeapon = Shooting.CurrentWeapon.Laser;
                GetComponent<Health>().healthImage.sprite = healthImages[1];
                abilties[1].enabled = true;
                GetComponent<Health>().armorLevel = 2;
                break;
            case 2:
                GetComponent<Shooting>().currentWeapon = Shooting.CurrentWeapon.Flamethrower;
                GetComponent<Health>().healthImage.sprite = healthImages[2];
                abilties[2].enabled = true;
                GetComponent<Health>().armorLevel = 3;
                break;
            default:
                break;
        }

        for (int i = 0; i < weaponObjects.Length; i++)
        {
            if (i == tankSelected)
            {
                weaponObjects[i].SetActive(true);
                tankModels[i].SetActive(true);
                anim.animator = weaponObjects[i].GetComponent<Animator>();
                anim.SetParameterAutoSend(0, true);
                anim.SetParameterAutoSend(1, true);
                turretWrangler.GetComponent<NetworkedMouseLook>().animator = anim.animator;
            }
            else
            {
                weaponObjects[i].SetActive(false);
            }
        }

        if (transform.position == Vector3.zero)
        {
            Debug.LogError("Reset the point");

            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        yield return new WaitUntil(GameManager.isInstanceNull);
        Transform _spawnPoint = GameManager.GetInstance.spawnPoints[ID].transform;
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
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
