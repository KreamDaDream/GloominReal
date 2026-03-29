using Unity.VisualScripting;
using UnityEngine;

public class OuchBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool active;
    void Update()
    {
        Collider2D[] stuff = Physics2D.OverlapBoxAll(transform.position, (GetComponent<Collider2D>().bounds.size), 0f);
        foreach (Collider2D col in stuff)
        {
            if (col.gameObject.name == "PLAYER" && active)
            {

                col.gameObject.GetComponent<plrMovement>().ihan.changeHealth(-1);

                break;
            }
        }
    }
}
