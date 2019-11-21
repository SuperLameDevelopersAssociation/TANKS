using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BasePickup : NetworkBehaviour
{
    public float destroyTime = 15f;

    void OnEnable()
    {
        //if (Time.timeSinceLevelLoad > 1)
            //StartCoroutine(Destroy(destroyTime));
    }

    IEnumerator Destroy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        NetworkServer.UnSpawn(gameObject);
        Destroy(gameObject);
    }
}
