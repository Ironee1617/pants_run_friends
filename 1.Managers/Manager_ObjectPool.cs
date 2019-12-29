using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_ObjectPool : MonoBehaviour {
    private static Manager_ObjectPool _instance;

    public static Manager_ObjectPool Instance
    {
        get
        {
            if(!_instance)
            {
                _instance = FindObjectOfType(typeof(Manager_ObjectPool)) as Manager_ObjectPool;
            }

            return _instance;
        }
    }

    public List<PooledObject> objectPool = new List<PooledObject>();

    void Start()
    {
        for(int i = 0; i < objectPool.Count; ++i)
        {
            objectPool[i].Initialize();
        }
    }

    public bool PushToPool(int itemID, GameObject item)
    {
        PooledObject pool = GetPoolItem(itemID);
        if (!pool)
            return false;

        pool.PushToPool(item);
        return true;
    }

    public GameObject PopFromPool(int itemID)
    {
        PooledObject pool = GetPoolItem(itemID);
        if (!pool)
            return null;
        
        return pool.PopFromPool();
    }

    PooledObject GetPoolItem(int itemID)
    {
        for(int i = 0; i < objectPool.Count; ++i)
        {
            if (objectPool[i].poolItemID.Equals(itemID))
            {
                return objectPool[i];
            }
        }

        return null;
    }
}
