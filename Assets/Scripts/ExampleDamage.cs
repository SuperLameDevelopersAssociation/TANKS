using UnityEngine;
using TrueSync;
using System.Collections;

public class ExampleDamage : TrueSyncBehaviour {

    //THIS SCRIPT IS HERE TO TEST HEALTH. DO NOT PLACE THIS IN THE SCENE IF YOU ARE NOT DOING THAT TEST
    public int damage;

    public void OnSyncedTriggerStay(TSCollision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Health hitPlayer = other.gameObject.GetComponent<Health>();
            if (hitPlayer.owner != owner)
            {
                hitPlayer.TakeDamage(this.tag);
            }
        }
    }

}
