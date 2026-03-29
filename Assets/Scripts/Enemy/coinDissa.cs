using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class coinDissa : MonoBehaviour
{
    public GameObject dissPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dissPoint != null)
        {
           if ((dissPoint.transform.position - transform.position).magnitude < 0.5)
            {
                GameObject.Find("InventoryHandler").GetComponent<IHan>().coin += 1;
                GetComponent<Rigidbody2D>().linearDamping = 2;
                AudioManager.Instance.PlaySFX(AudioManager.Instance.coin);

                gameObject.SetActive(false);
            }
                    
        }
    }
}
