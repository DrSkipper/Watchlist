using UnityEngine;
using System.Collections.Generic;

public class ObjectPools : MonoBehaviour
{
    public static ObjectPools Instance { get { return _instance; } }

    public ObjectPool[] Pools;

    [System.Serializable]
    public struct ObjectPool
    {
        public string Key;
        public GameObject Prefab;
        public int MaxToStore;
        public int CreatePerSecond;
    }

    void Awake()
    {
        _pooledObjects = new Dictionary<string, List<GameObject>>();
        _keyToPoolIndex = new Dictionary<string, int>();
        for (int i = 0; i < this.Pools.Length; ++i)
        {
            _pooledObjects.Add(this.Pools[i].Key, new List<GameObject>());
            _keyToPoolIndex.Add(this.Pools[i].Key, i);
        }
        _instance = this;
    }

    void OnDestroy()
    {
        _instance = null;
    }

    public GameObject GetPooledObject(string key)
    {
        if (_pooledObjects.ContainsKey(key))
        {
            return getObject(_pooledObjects[key], this.Pools[_keyToPoolIndex[key]].Prefab);
        }
        return null;
    }

    public void ReturnPooledObject(string key, GameObject go)
    {
        if (_pooledObjects.ContainsKey(key))
        {
            returnObject(_pooledObjects[key], go, this.Pools[_keyToPoolIndex[key]].MaxToStore);
        }
    }

    /**
     * Private
     */
    private Dictionary<string, List<GameObject>> _pooledObjects;
    private Dictionary<string, int> _keyToPoolIndex;
    private static ObjectPools _instance;

    public GameObject getObject(List<GameObject> list, GameObject prefab)
    {
        if (list.Count > 0)
        {
            GameObject retVal = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            retVal.SetActive(true);
            return retVal;
        }
        else
        {
            return Instantiate(prefab) as GameObject;
        }
    }

    public void returnObject(List<GameObject> list, GameObject go, int maxToStore)
    {
        if (list.Count < maxToStore)
        {
            go.SetActive(false);
            list.Add(go);
        }
        else
        {
            Destroy(go);
        }
    }
}
