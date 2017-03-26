using UnityEngine;
using UnityEngine.Networking;
//using System.Collections;

public class PlayerSetup : NetworkBehaviour 
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    //[HideInInspector]
    public byte ID;

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
    }

    [ClientRpc]
    public void RpcSetID(byte myID)
    {
        Debug.LogError("Set ID: " + myID);
        ID = myID;
        GetComponent<Health>().ID = myID;
        GetComponent<Shooting>().ID = myID;
    }
}
