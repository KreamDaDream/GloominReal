using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjPool : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private List<GameObject> pooledObjects = new List<GameObject>();
    [SerializeField] private GameObject obj;
    [SerializeField] private int amtToPool;
    private void Awake()
    {
      
    }

    void Start()
    {
        for (int i = 0; i < amtToPool; i++)
        {
            GameObject o = Instantiate(obj);
            o.transform.parent = gameObject.transform;
            o.SetActive(false);
            pooledObjects.Add(o);
        }
    }

    public GameObject getObj()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
