using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class daShop : MonoBehaviour
{

    [SerializeField] private List<string> sells;
    [SerializeField] private List<int> prices;

    public Vector3 area;
    [SerializeField] private List<GameObject> slots;

    [SerializeField] private GameObject temp;
    [SerializeField] private TextMeshProUGUI coinText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       reload();
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = IHan.instance.coin.ToString();
    }

    public void reload()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Destroy(slots[i]);
        }
        slots.Clear();
            for (int i = 0; i < sells.Count; i++)
        {

            GameObject slot = Instantiate(temp);
            slot.GetComponent<slotData>().item = sells[i];
            slot.GetComponent<slotData>().price = prices[i];
            slot.transform.SetParent(temp.transform.parent, false);
            slot.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = sells[i] + " - $" + prices[i];
            slots.Add(slot);
            slot.GetComponent<Button>().onClick.AddListener(() => Buy(slot.GetComponent<slotData>()));
            slot.SetActive(true);

        }
    }

    public void Buy(slotData dat)
    {
        if (IHan.instance.inventory.Count < 8)
        {
            if (IHan.instance.coin >= dat.price)
            {
                IHan.instance.coin -= dat.price;

                IHan.instance.inventory.Add(dat.item);
            }


        }
    }

}
