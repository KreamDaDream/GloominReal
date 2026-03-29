using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite activated;
    [SerializeField] private Sprite deactivated;

    public bool source;
    public bool active = false;
    public bool connectedToSource;

    private float[] rots = {0, 90, 180, -90};
    private int ind;
    public List<GameObject> points;
    private bool shuffed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (!shuffed && Time.time > 1)
        {
            if (!gameObject.transform.parent.gameObject.GetComponent<PipeSystem>().done)
            {
                shuffed = true;
                if (!source)
                {
                    print(Time.time);

                    ind = Random.Range(0, rots.Length);
                    transform.rotation = Quaternion.Euler(0, 0, rots[ind]);
                }
            }
        }

        if (active)
        {
            GetComponent<SpriteRenderer>().sprite = activated;
        } else
        {
            GetComponent<SpriteRenderer>().sprite = deactivated;

        }
       
      
        if (source)
        {
            active = true;
        }
    }


    public void OnInteract()
    {
        if (!source)
        {
            ind++;
            if (ind > 3)
            {
                ind = 0;
            }
            AudioManager.Instance.PlaySFX(AudioManager.Instance.pipe);
            transform.rotation = Quaternion.Euler(0, 0, rots[ind]);

        }
    }
}
