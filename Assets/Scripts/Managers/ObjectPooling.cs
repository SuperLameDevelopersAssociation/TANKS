using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TrueSync;

public class ObjectPooling : TrueSyncBehaviour
{
    public GameObject[] pooledObject;
    public int pooledAmount;
    public float waitTime = 0.001f;
    public bool willGrow = true;

    FP _waitTime;
    public List<GameObject> pooledObjects;
    
    public override void OnSyncedStart()
    {
        _waitTime = waitTime;
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < pooledAmount; i++)
        {
            TrueSyncManager.SyncedStartCoroutine(InstantiatePool(waitTime));
            //GameObject obj = (GameObject)Instantiate(pooledObject);
            //obj.SetActive(false);
            //pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (willGrow)
        {
            for (int j = 0; j < pooledObject.Length; j++)
            {
                GameObject obj = TrueSyncManager.SyncedInstantiate(pooledObject[j]);
                obj.transform.parent = transform;
                pooledObjects.Add(obj);
                return obj;
            }
        }

        return null;
    }

    IEnumerator InstantiatePool(float waitTime)
    {
        for (int k = 0; k < pooledObject.Length; k++)
        {
            GameObject obj = TrueSyncManager.SyncedInstantiate(pooledObject[k]);
            obj.transform.parent = transform;

            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        yield return _waitTime;
    }
}