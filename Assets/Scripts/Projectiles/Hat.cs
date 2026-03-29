using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<Sprite> hats;
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = hats[Random.Range(0, hats.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
