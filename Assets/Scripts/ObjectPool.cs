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
            ob.GetComponent<PoolObject>().InitializeEffect(prefab);
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
            ob.GetComponent<PoolObject>().InitializeEffect(prefab);
            return ob.transform;
        }
    }


    public void ReturnObject(GameObject prefab, GameObject returning)
    {
        returning.SetActive(false);
        _pools[prefab].Enqueue(returning);
    }



    /* ==================== Class ==================== */

    /// <summary>
    /// All pool objects must inherit this class.
    /// </summary>
    public abstract class PoolObject : MonoBehaviour
    {
        private GameObject _prefab = null;


        public void InitializeEffect(GameObject prefab)
        {
            _prefab = prefab;
        }


        protected void ReturnObject()
        {
            StageManagerBase.ObjectPool.ReturnObject(_prefab, gameObject);
        }
    }
}
