using UnityEngine;
using System.Collections;

public class WaterDeath : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            other.GetComponent<Health>().RpcHasDied();
        }
    }
}
