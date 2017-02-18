//Christopher Koester  AKA  ¡el héroe!
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TrueSync;

public class PoolManagerScript : TrueSyncBehaviour
{
    //ceating the dictionary that will allow us to cycle through our objects
    //from a specifically strict order (i.e. 1,2,3,1,2,3,1,2,3)
    Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

    static PoolManagerScript _instance; //class

    public static PoolManagerScript instance//function
    {  
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PoolManagerScript>();
            }
            return _instance;
        }//end of get
    }//end of function

    public void CreatePool(GameObject prefab, int poolSize)
    {//creating our prefabs, and poolsize
        int poolKey = prefab.GetInstanceID();

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

            GameObject poolHolder = new GameObject(prefab.name + " pool");
            poolHolder.transform.parent = transform;

            for (int i = 0; i < poolSize; i++)
            {//instantiating the objects through this loop based on how large the int, poolSize, is.
                ObjectInstance newObject = new ObjectInstance(TrueSyncManager.SyncedInstantiate(prefab) as GameObject);
              //  prefab.AddComponent<TSTransform>();
                poolDictionary[poolKey].Enqueue(newObject);
                newObject.SetParent(poolHolder.transform);
            }//end of forloop
        }//end of if
    }//end of function
    public void ReuseObject(GameObject prefab, TSVector position, TSVector direction, TSQuaternion rotation)
    {//The oldest object from the pool is re-used as the current one
        //in the example of 1,2,3, 1 was oldest (as it was instantiated first) so it would be the next one instantiated
        //1,2,3, 1,2,3,1,2,3, etc.
        int poolKey = prefab.GetInstanceID();
        if (poolDictionary.ContainsKey(poolKey))
        {
            ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(objectToReuse);
            objectToReuse.Reuse(position, direction, rotation);
        }//end of if
    }//end of function

    public class ObjectInstance
    {
        GameObject gameObject;
        Transform transform;

        bool hasPoolObjectComponent;
        PoolObject poolObjectScript;

        public ObjectInstance(GameObject objectInstance)
        {
            gameObject = objectInstance;
            transform = gameObject.transform;
            gameObject.SetActive(false);

            if (gameObject.GetComponent<PoolObject>())
            {
                hasPoolObjectComponent = true;
                poolObjectScript = gameObject.GetComponent<PoolObject>();
            }//end of if
        }//end of function
        public void Reuse(TSVector position, TSVector direction, TSQuaternion rotation)
        {
            //turning the object on and assiging the transforms
            gameObject.SetActive(true);
            gameObject.GetComponent<TSTransform>().position = position;
            gameObject.GetComponent<TSTransform>().rotation = rotation;
            gameObject.GetComponent<Projectile>().actualDirection = direction;


            //if (hasPoolObjectComponent)
            //{
            //   poolObjectScript.OnObjectReuse();
            //}//end of if
        }//end of function
        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }//end of function
    }//end of inner class
}//end of outer class