using UnityEngine;

public class phyShop : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject shop;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shop.activeInHierarchy)
        {
            if ((shop.GetComponent<daShop>().area - plrMovement.instance.gameObject.transform.position).magnitude > 3)
            {
                shop.SetActive(false);
            }
        }
    }

    public void OnInteract()
    {
        shop.SetActive(true);
        shop.GetComponent<daShop>().area = transform.position;
        shop.GetComponent<daShop>().reload();
    }
}
