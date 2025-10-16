using UnityEngine;
using System.Collections.Generic;

public abstract class GenericPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T _prefab;
    [SerializeField] protected int _poolSize = 20;
    [SerializeField] protected Transform _container;

    protected Queue<T> _pool = new Queue<T>();
    protected List<T> _activeObjects = new List<T>();

    protected virtual void Awake()
    {
        InitializePool();
    }

    public int GetActiveObjectsCount() =>
        _activeObjects.Count;

    public int GetPooledObjectsCount() =>
        _pool.Count;

    public virtual T GetObject(Vector3 position)
    {
        if (_prefab == null)
            return null;

        T obj;

        if (_pool.Count > 0)
            obj = _pool.Dequeue();
        else
            obj = CreateNewObject();

        if (obj != null)
        {
            obj.transform.position = position;
            obj.gameObject.SetActive(true);
            _activeObjects.Add(obj);
        }

        return obj;
    }

    public virtual void ReturnObject(T obj)
    {
        if (obj == null)
            return;

        obj.gameObject.SetActive(false);
        _activeObjects.Remove(obj);
        _pool.Enqueue(obj);
    }

    public virtual void ResetPool()
    {
        foreach (var obj in _activeObjects.ToArray())
        {
            if (obj != null)
                ReturnObject(obj);
        }
    }

    protected virtual void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            var obj = CreateNewObject();
            if (obj != null)
                _pool.Enqueue(obj);
        }
    }

    protected virtual T CreateNewObject()
    {
        if (_prefab == null)
            return null;

        var obj = Instantiate(_prefab, _container != null ? _container : transform);
        obj.gameObject.SetActive(false);
        return obj;
    }
}