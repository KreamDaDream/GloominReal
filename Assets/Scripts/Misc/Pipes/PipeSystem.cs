using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PipeSystem : MonoBehaviour, IDataPersistence
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private List<GameObject> pipes = new List<GameObject>();
    private Dictionary<GameObject, bool> connectors = new Dictionary<GameObject, bool>();
    private GameObject source;
    private float lastCheck = 0;
    [SerializeField] private GameObject door;

    [SerializeField] private Sprite open;
    [SerializeField] private Sprite close;
    [SerializeField] private GameObject leavePipe;
    public bool done = false;
    public void LoadData(GameData data)
    {
        done = data.Solved;
        
    }

    public void SaveData(ref GameData data)
    {

        data.Solved = isOpen;

    }








    public bool isOpen = false; 
    void Start()
    {
        if (done)
        {
            leavePipe.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        foreach (Transform p in transform)
        {
            pipes.Add(p.gameObject);
        }

        for (int i = 0; i < pipes.Count; i++)
        {
            if (pipes[i].GetComponent<Pipe>().source)
            {
                source = pipes[i];
            }
            foreach (GameObject con in pipes[i].GetComponent<Pipe>().points)
            {
                connectors.Add(con, false);
            }
        }
    }

    // Update is called once per frame

   

    void Tracker(GameObject startPipe)
    {
        bool atEnd = false;
        GameObject curPipe = startPipe;

        while (!atEnd)
        {
            List<GameObject> points = curPipe.GetComponent<Pipe>().points;
            int frees = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (!connectors[points[i]])
                {
                    frees++;
                    connectors[points[i]] = true;
                    Collider2D[] stuff = Physics2D.OverlapBoxAll(points[i].transform.position, new Vector2(0.5f, 0.5f), 0f);
                    foreach (Collider2D col in stuff)
                    {
                        if (col.gameObject != points[i])
                        {
                            if (col.gameObject.name.Contains("pipePoint"))
                            {
                                connectors[col.gameObject.transform.parent.parent.gameObject] = true;
                                col.gameObject.transform.parent.parent.gameObject.GetComponent<Pipe>().active = true;
                                Tracker(col.gameObject.transform.parent.parent.gameObject);
                            }
                        }
                    }
                }
            }
            if (frees == 0)
            {
                atEnd = true;
            }
        }
    }

    
    void Update()
    {
        
        if (Time.time - lastCheck > 0.5f)
        {
            for (int i = 0; i < pipes.Count; i++)
            {
                pipes[i].GetComponent<Pipe>().active = false;
            }
            var list = connectors.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                connectors[list[i].Key] = false;
            }
            Tracker(source);
            lastCheck = Time.time;
        }
        if (leavePipe.GetComponent<Pipe>().active)
        {
            isOpen = true;
        } else
        {
            isOpen = false;

        }
        if (isOpen)
        {
            door.GetComponent<SpriteRenderer>().sprite = open;
            door.GetComponent<BoxCollider2D>().enabled = false;

        }
        else
        {
            door.GetComponent<SpriteRenderer>().sprite = close;
            door.GetComponent<BoxCollider2D>().enabled = true;

        }
    }
}
