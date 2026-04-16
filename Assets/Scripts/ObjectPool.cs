using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private static ObjectPool instance;
    public static ObjectPool Instance => instance ??= new ObjectPool();

    // 用“预制件名字”（不含 Clone）当 key
    private Dictionary<string, Queue<GameObject>> objectPool = new();

    // 根节点：用来把所有池子归到同一个 GameObject 下
    private GameObject poolRoot;

    private ObjectPool()
    {
        poolRoot = new GameObject("ObjectPool");
        Object.DontDestroyOnLoad(poolRoot);
    }

    /// <summary>
    /// 从池里拿，如果池里没空闲的才 Instantiate
    /// </summary>
    public GameObject GetObject(GameObject prefab)
    {
        string key = prefab.name;

        // 如果有可复用对象
        if (objectPool.ContainsKey(key) && objectPool[key].Count > 0)
        {
            var obj = objectPool[key].Dequeue();
            obj.SetActive(true);
            return obj;
        }

        // 否则新建
        var newObj = GameObject.Instantiate(prefab);
        newObj.name = key;  // 去掉 "(Clone)"
        EnsureChildPoolExists(key);
        newObj.transform.SetParent(poolRoot.transform.Find(key + "Pool"), false);
        return newObj;
    }

    /// <summary>
    /// 回收对象：SetActive(false) 并入队
    /// </summary>
    public void ReleaseObject(GameObject obj)
    {
        string key = obj.name;
        if (!objectPool.ContainsKey(key))
            objectPool[key] = new Queue<GameObject>();

        obj.SetActive(false);
        objectPool[key].Enqueue(obj);
    }

    /// <summary>
    /// 确保有一个叫 “{key}Pool” 的子物体挂在根节点下
    /// </summary>
    private void EnsureChildPoolExists(string key)
    {
        var child = poolRoot.transform.Find(key + "Pool");
        if (child != null) return;

        var go = new GameObject(key + "Pool");
        go.transform.SetParent(poolRoot.transform, false);
    }
}
