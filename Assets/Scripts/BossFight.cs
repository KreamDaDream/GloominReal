using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class BossFight : MonoBehaviour, IDataPersistence
{
    public static BossFight instance;
    private bool trig = false;
    [SerializeField] private GameObject battleBox;
    [SerializeField] private GameObject ob;
    [SerializeField] private GameObject loot;
    public bool ht;
    public bool d;

    private bool check = false;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!check)
        {


            if (Time.time > 0.5f)
            {
                check = true;
                if (d)
                {
                    Destroy(ob);
                    if (!ht)
                    {   
                        loot.SetActive(true);
                    }
                }
                if (ht)
                {
                    Destroy(loot);
                }
            }
        }
    }

    public void LoadData(GameData data)
    {
        d = data.bossKilled;
        ht = data.hatTaken;
    }

    public void SaveData(ref GameData data)
    {
        data.bossKilled = d;
        data.hatTaken = ht;

    }



    IEnumerator talk()
    {
       

        plrMovement.instance.canOpenMenu = false;
        plrMovement.instance.canMove = false;
        AudioManager.Instance.ChangeMusic(AudioManager.Instance.wind);
        plrMovement.instance.dir = new Vector3(1, 0);
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(UIHandler.instance.speak("Oh HELLOOOO!!!!!!!`````` Are you here to see my collection?", "Hatted Gloom", ob));
        yield return StartCoroutine(UIHandler.instance.speak("I'm here to kill you,````` you scourge of the Earth.", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("You know,``` you ran away so fast from the village, I didn't even get to thank you.", "Hatted Gloom", ob));
        yield return StartCoroutine(UIHandler.instance.speak("Without your help, I wouldn't have been able to get all these cool hats!", "Hatted Gloom", ob));
        yield return StartCoroutine(UIHandler.instance.speak("shut up.", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("What's with the change of tune?````` I thought we were``` BEST ````friends!", "Hatted Gloom", ob));
        yield return StartCoroutine(UIHandler.instance.speak("You killed my people.", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("...`````Was that not what you wanted?", "Hatted Gloom", gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("What you```` paid for?", "Hatted Gloom", gameObject));

        yield return StartCoroutine(UIHandler.instance.speak("You're`````` dead.", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("Oh well,````` your helmet will look good with all my others!", "Hatted Gloom", ob));

        battleBox.SetActive(true);
        StartCoroutine(battleBox.GetComponent<BattleBox>().toggleBox(true, ob));
        yield return new WaitForSeconds(2f);
        loot.SetActive(true);
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (!d)
        {
            if (collision.gameObject == plrMovement.instance.gameObject)
            {
                if (!trig)
                {
                    trig = true;

                    StartCoroutine(talk());
                }
            }
        }
    }
}
