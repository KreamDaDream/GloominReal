using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class IHan : MonoBehaviour
{
    public static IHan instance;
    public class itemInfoC
    {
        public string description;
        public int index;
        public bool Ew;
        public bool Ed;

        public itemInfoC(string description, int index, bool Ew = false, bool Ed = false)
        {
            this.description = description;
            this.index = index;
            this.Ew = Ew;
            this.Ed = Ed;
        }
    }
    public bool titleCard = false;
    public GameObject inventoryUI;
    public int slotSelected = 0;
    public GameObject physicalSlot;
    
    public GameObject combatInvent;

    public List<Sprite> texes = new List<Sprite>();
    [SerializeField] private GameObject uiList;
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject icon;

    [SerializeField] private GameObject healthText;
    [SerializeField] private GameObject coinText;


    [SerializeField] private GameObject battleCoinText;

    [SerializeField] private GameObject weaponSlot;
    [SerializeField] private GameObject defenseSlot;

    [SerializeField] private GameObject comInvTemp;

    public Dictionary<string, itemInfoC> itemInfo = new Dictionary<string, itemInfoC>()
    {
        {"Sword", new itemInfoC("Everyday blade for everyday stabbing. 2 AT", 0, true, false) },
        {"Chainmail Shirt", new itemInfoC("Protective shirt that goes UNDER the leather one. 2 DF", 1, false, true) },
        {"Chocolate Covered Leaves", new itemInfoC("Now with 20% more chocolate! 3 HP", 2) },
        {"Leaf Covered Chocolate", new itemInfoC("Now with 20% more leaf! 5 HP", 3) },
        {"Assorted Berries", new itemInfoC("Your mother wouldn't approve of eating these. Has a 50/50 chance to heal or damage.", 4) },
        {"Health Potion", new itemInfoC("Generic and boring. 10 HP", 5) },
        {"Tree's Vengeance", new itemInfoC("We are tired of being cut down! ULTIMATE PAPER CUT! 4 AT", 6, true, false) },
        {"Pirate Hat", new itemInfoC("Worn from the battle.", 7, false, false) }

    };

    public Dictionary<string, int> weaponStats = new Dictionary<string, int>()
    {
        { "Sword", 1 },
        { "Tree's Vengeance", 2 }
    };

    public Dictionary<string, int> armourStats = new Dictionary<string, int>()
    {
        { "Chainmail Shirt", 2 }
    };

    public string[] inCombatItems =
    {
        "Chocolate Covered Leaves", "Leaf Covered Chocolate", "Assorted Berries", "Health Potion"
    };
    public int maxHealth;
    public int health;

    public int coin;

    public int attackPoints;
    public int defensePoints;

    public GameObject APBar;

    [SerializeField] private float rechargeRate;
    private float lastRecharge;

    public int AP;
    public string equippedW;
    public string equippedD;
    [SerializeField] private TextMeshProUGUI battleHealth;

    public List<string> inventory = new List<string>();

    private float lastDam = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastRecharge > rechargeRate)
        {
            lastRecharge = Time.time;
            if (AP < 5)
            {
                AP++;
            }
        }
        for (int j = 0; j < 5; j++)
        {
            if ((j + 1) <= AP)
            {
                APBar.transform.GetChild(j).gameObject.SetActive(true);
            } else
            {
                APBar.transform.GetChild(j).gameObject.SetActive(false);
            }
            
        }

        defenseSlot.GetComponent<TextMeshProUGUI>().text = equippedD;
        weaponSlot.GetComponent<TextMeshProUGUI>().text = equippedW;

        healthText.GetComponent<TextMeshProUGUI>().text = health + "/" + maxHealth;
        int i = 0;
        foreach (Transform slot in uiList.transform)
        {
            if (inventory.Count - 1 >= i)
            {
                slot.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text = inventory[i];
            }
            else
            {
                slot.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text = "";
            }
            i++;
        }
        if (physicalSlot.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text != "")
        {
            icon.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            textBox.GetComponent<TextMeshProUGUI>().text = itemInfo[physicalSlot.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text].description;
            icon.GetComponent<Image>().sprite = texes[itemInfo[physicalSlot.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text].index];
        }
        else
        {
            textBox.GetComponent<TextMeshProUGUI>().text = "";
            icon.GetComponent<Image>().sprite = null;
            icon.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
        List<string> combatInvStuff = new List<string>();

        for (int k = 0; k < inventory.Count; k++)
        {
            if (inventory[k] == "")
            {
                inventory.RemoveAt(k);
            }
        }

        foreach (var item in inventory)
        {
            
            if (Contains(inCombatItems, item))
            {
                combatInvStuff.Add(item);
            }
            
        }

        
        foreach (Transform slot in combatInvent.transform)
        {
            slot.gameObject.SetActive(false);
        }

        for (int j = 0; j < combatInvStuff.Count; j++)
        {
            combatInvent.transform.GetChild(j).gameObject.SetActive(true);
            combatInvent.transform.GetChild(j).Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = combatInvStuff[j].ToString();

        }
        if (AP < 2)
        {
            for (int j = 0; j < combatInvent.transform.childCount; j++)
            {
                combatInvent.transform.GetChild(j).GetComponent<Button>().interactable = false;

            }
        } else
        {
            for (int j = 0; j < combatInvent.transform.childCount; j++)
            {
                combatInvent.transform.GetChild(j).GetComponent<Button>().interactable = true;

            }

        }
            battleHealth.text = health + "/" + maxHealth;
            coinText.GetComponent<TextMeshProUGUI>().text = coin.ToString();
            battleCoinText.GetComponent<TextMeshProUGUI>().text = coin.ToString();
            if (equippedW != "")
        {
            attackPoints = weaponStats[equippedW];

        }
        if (equippedD != "")
        {
            defensePoints = armourStats[equippedD];

        }
    }

    public void select(GameObject slot)
    {
        if (slot.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text == "") { return; }

        slotSelected = 0;
        physicalSlot = slot;
        foreach (Transform sloT in uiList.transform)
        {
            if (sloT.gameObject == slot)
            {
                break;
            }
            slotSelected++;
        }
        textBox.GetComponent<TextMeshProUGUI>().text = itemInfo[slot.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text].description;
        icon.GetComponent<Image>().sprite = texes[itemInfo[slot.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text].index];
    }

    public void AddItem(string Name)
    {
        inventory.Add(Name);
    }

    public void Drop()
    {
        if (inventory[slotSelected] != "Pirate Hat")
        {
            GameObject bag = Instantiate(Resources.Load<GameObject>("Prefs/ItemBag"));
            bag.transform.parent = null;
            bag.transform.position = GameObject.Find("PLAYER").transform.position;
            bag.GetComponent<Bag>().content = inventory[slotSelected];
            inventory.RemoveAt(slotSelected);
        }

    }

    public void Interact()
    {
        if (itemInfo[inventory[slotSelected]].Ew)
        {
            string oldWeapon = equippedW;
            equippedW = inventory[slotSelected];
            if (oldWeapon != "")
            {
                inventory[slotSelected] = oldWeapon;
            } else
            {
                inventory.RemoveAt(slotSelected);
            }
                attackPoints = weaponStats[equippedW];
            weaponSlot.GetComponent<TextMeshProUGUI>().text = equippedW;

        } else if (itemInfo[inventory[slotSelected]].Ed)
        {
            string oldArmour = equippedD;
            equippedD = inventory[slotSelected];
            if (oldArmour != "")
            {
                inventory[slotSelected] = oldArmour;
            }
            else
            {
                inventory.RemoveAt(slotSelected);
            }
            defensePoints = armourStats[equippedD];
            defenseSlot.GetComponent<TextMeshProUGUI>().text = equippedD;

        }
        else
        {
            if (inventory[slotSelected] == "Chocolate Covered Leaves")
            {
                changeHealth(3);
                inventory.RemoveAt(slotSelected);
            } else if (inventory[slotSelected] == "Leaf Covered Chocolate")
            {
                changeHealth(5);
                inventory.RemoveAt(slotSelected);

            }
            else if (inventory[slotSelected] == "Assorted Berries")
            {
                changeHealth(Mathf.Clamp(Mathf.Clamp(Random.Range(-5, 20), -5, 10), (health * -1) + 1, maxHealth));
                inventory.RemoveAt(slotSelected);

            }
            else if (inventory[slotSelected] == "Health Potion")
            {
                changeHealth(10);
                inventory.RemoveAt(slotSelected);

            }
        }
    }

    public void BattleItem(GameObject slot)
    {
        if (requestAP(2))
        {
            string item = slot.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text;
            if (item == "Chocolate Covered Leaves")
            {
                changeHealth(3);
                inventory.Remove(item);
            }
            else if (item == "Leaf Covered Chocolate")
            {
                changeHealth(5);
                inventory.Remove(item);

            }
            else if (item == "Assorted Berries")
            {
                changeHealth(Mathf.Clamp(Mathf.Clamp(Random.Range(-5, 5), -5, 10), (health * -1) + 1, maxHealth));
                inventory.Remove(item);

            }
            else if (item == "Health Potion")
            {
                changeHealth(10);
                inventory.Remove(item);

            }
        }
       }

    public void changeHealth(int healthAmt)
    {
        if (healthAmt < 0)
        {
            if (!titleCard)
            {
                if (Time.time - lastDam > 1f)
                {
                    lastDam = Time.time;
                    GameObject.Find("PLAYER").GetComponent<plrMovement>().shake = 0.2f;
                    health = Mathf.Clamp(health + healthAmt, 0, maxHealth);
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.hurt);
                }
                if (health == 0 && !GameObject.Find("PLAYER").GetComponent<plrMovement>().dying)
                {
                    GameObject.Find("PLAYER").GetComponent<plrMovement>().die();
                }
            }
        } else
        {
            
            health = Mathf.Clamp(health + healthAmt, 0, maxHealth);
            
        }
    }

    public bool Contains(string[] ray, string item)
    {
        foreach (string rayItem in ray)
        {
            if (rayItem == item)
            {
                return true;
            }
        }
        return false;
    }

    public bool requestAP(int amt = 1)
    {
        if (AP - amt >= 0)
        {
            lastRecharge = Time.time;
            AP -= amt;
            return true;
        } else
        {
            return false;
        }
    }
}