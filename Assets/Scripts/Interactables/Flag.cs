using UnityEngine;

public class Flag : MonoBehaviour, IInteractable
{

    private GameObject plr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (plr == null)
        {
            plr = GameObject.Find("PLAYER");
        }   
    }

    public void OnInteract()
    {
        plr.GetComponent<plrMovement>().checky(gameObject);
    }

}
