
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    public GameObject Prefab;

    private Queue<GameObject> pool = new Queue<GameObject>();

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        if (pool.Count == 0)
            AddGameObject(1);

        var go = pool.Dequeue();

#if UNITY_EDITOR
        go.transform.SetParent(null);
#endif

        go.transform.SetPositionAndRotation(position, rotation);
        go.SetActive(true);
        return go;
    }

    public void ReturnToPool(GameObject go)
    {
        go.SetActive(false);

#if UNITY_EDITOR
        go.transform.SetParent(transform);
#endif

        pool.Enqueue(go);
    }

    private void AddGameObject(int count)
    {
#if UNITY_EDITOR
        var go = Instantiate(Prefab, transform, true);
#else
        var go = Instantiate(Prefab);
#endif

        go.SetActive(false);
        go.GetComponent<IPoolable>().Pool = this;
        pool.Enqueue(go);
    }
}

public interface IPoolable
{
    GameObjectPool Pool { get; set; }
}
