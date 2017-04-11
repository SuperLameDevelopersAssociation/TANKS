using UnityEngine;
using UnityEngine.Networking;
//using System.Collections;

public class PlayerSetup : NetworkBehaviour 
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    //[HideInInspector]
    [SyncVar]
    public byte ID;
    [HideInInspector]
    public Transform spawnPoint;

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

        //Debug.LogError(transform.position);

        //if(transform)
    }
}
