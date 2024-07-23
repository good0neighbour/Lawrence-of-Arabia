using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    /* ==================== Fields ==================== */

    private Dictionary<GameObject, Queue<GameObject>> _pools = new Dictionary<GameObject, Queue<GameObject>>();
    private Transform _parent = null;



    /* ==================== Public Methods ==================== */

    public ObjectPool(Transform poolParent)
    {
        _parent = poolParent;
    }


    public void PoolPreparing(GameObject prefab)
    {
        if (_pools.ContainsKey(prefab))
        {
            return;
        }
        else
        {
            GameObject ob = Object.Instantiate(prefab, _parent);
            ob.SetActive(false);
            ob.GetComponent<PoolObject>().PrefabType = prefab;
            _pools.Add(prefab, new Queue<GameObject>());
            _pools[prefab].Enqueue(ob);
        }
    }


    public Transform GetObject(GameObject prefab)
    {
        GameObject result;

        if (_pools[prefab].Count > 0)
        {
            result = _pools[prefab].Dequeue();
            result.SetActive(true);
            return result.transform;
        }
        else
        {
            GameObject ob = Object.Instantiate(prefab, _parent);
            ob.GetComponent<PoolObject>().PrefabType = prefab;
            return ob.transform;
        }
    }


    public void ReturnObject(GameObject prefab, GameObject returning)
    {
        returning.SetActive(false);
        _pools[prefab].Enqueue(returning);
    }



    /* ==================== Interface ==================== */

    /// <summary>
    /// All pool objects must inherit this interface.
    /// </summary>
    public interface PoolObject
    {
        public GameObject PrefabType
        {
            get;
            set;
        }
    }
}
