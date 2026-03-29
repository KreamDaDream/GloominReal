using UnityEngine;

public class Bag: MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string content = "";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (content != "")
        {
            Sprite img = IHan.instance.texes[IHan.instance.itemInfo[content].index];
            GetComponent<SpriteRenderer>().sprite = img;
        }
    }

    public void OnInteract()
    {
        if (IHan.instance.inventory.Count < 8)
        {
            IHan.instance.AddItem(content);
            if (content == "Pirate Hat")
            {
                BossFight.instance.ht = true;

            }
            Destroy(gameObject);
        }
    }

 }
