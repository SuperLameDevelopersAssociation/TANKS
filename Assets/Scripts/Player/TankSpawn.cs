using UnityEngine;
using System.Collections;
using TrueSync;

public class TankSpawn : TrueSyncBehaviour
{
    public void OnSyncedTriggerExit(TSCollision other)
    {
        if (other.gameObject.tag == "Spawn")
        {
            this.GetComponent<Health>().inSpawn = false;
            print(this.GetComponent<Health>().inSpawn + " : for we have left spawn");
        }
    }
}
