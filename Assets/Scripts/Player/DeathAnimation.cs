using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class DeathAnimation : NetworkBehaviour
{
    public GameObject[] objectsToDestory;
    public GameObject explosion;

    [Command]
    public void CmdSetTankState(bool isFunctional)
    {
        SetTankState(isFunctional);
        RpcSetTankState(isFunctional);
    }

    [ClientRpc]
    void RpcSetTankState(bool isFunctional)
    {
        SetTankState(isFunctional);
    }

    void SetTankState(bool isFunctional)
    {
        foreach (GameObject obj in objectsToDestory)
        {
            obj.SetActive(isFunctional);
        }
        explosion.SetActive(!isFunctional);
    }    
}
