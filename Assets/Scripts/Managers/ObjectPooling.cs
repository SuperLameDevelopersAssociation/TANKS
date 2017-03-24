using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooling : MonoBehaviour
{
    public GameObject[] pooledObject;
    public int pooledAmount;
    public bool willGrow = true;

    public List<GameObject> pooledObjects;
    
    void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < pooledAmount; i++)
        {
            InstantiatePool();
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
                GameObject obj = NetworkManager.Instantiate(pooledObject[j], Vector3.zero, Quaternion.identity) as GameObject;
                obj.transform.parent = transform;
                pooledObjects.Add(obj);
                return obj;
            }
        }

        return null;
    }

    void InstantiatePool()
    {
        for (int k = 0; k < pooledObject.Length; k++)
        {
            GameObject obj = NetworkManager.Instantiate(pooledObject[k], Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.parent = transform;

            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }
}