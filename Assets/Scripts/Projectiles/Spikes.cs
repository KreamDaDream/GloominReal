using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Spikes : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject spike;
    [SerializeField] private Sprite up;
    [SerializeField] private Sprite down;
    public bool icon = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator cycle(float dur)
    {
        if (icon)
        {
            spike.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
            spike.GetComponent<SpriteRenderer>().sprite = up;
            yield return new WaitForSeconds(dur);
            spike.GetComponent<SpriteRenderer>().sprite = down;

        }
        else
        {
            spike.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1f);

            spike.GetComponent<Animator>().SetBool("up", true);
            spike.GetComponent<Animator>().SetTrigger("Play");
            yield return new WaitForSeconds(0.517f);
            spike.GetComponent<SpriteRenderer>().sprite = up;
            yield return new WaitForSeconds(dur);
            spike.GetComponent<Animator>().SetBool("up", false);
            spike.GetComponent<Animator>().SetTrigger("Play");
            yield return new WaitForSeconds(0.517f);
            spike.GetComponent<SpriteRenderer>().sprite = down;
        }
        gameObject.SetActive(false);

    }
}
