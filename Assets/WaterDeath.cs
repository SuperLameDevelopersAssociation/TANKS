using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WaterDeath : NetworkBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            other.GetComponent<Health>().RpcHasDied();
        }
    }
}
