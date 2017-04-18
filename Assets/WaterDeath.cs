using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WaterDeath : NetworkBehaviour
{
    [Server]
    void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            other.GetComponent<Health>().RpcHasDied();
        }
    }
}
