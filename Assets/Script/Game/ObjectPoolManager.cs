using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance = null;

    private List<GameObject> pooledObjects = new List<GameObject> ();
    private int amountToPool = 50;

    [SerializeField] private GameObject shellPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(shellPrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
