using UnityEngine;
using System.Collections;
public class pirate : MonoBehaviour, IInteractable, IDataPersistence
{

    private bool talkedTo;
    [SerializeField] private Sprite bald;
    [SerializeField] private Sprite hatted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnInteract()
    {
        StartCoroutine(seq());
    }

    IEnumerator seq()
    {
        if (IHan.instance.inventory.Contains("Pirate Hat"))
        {
            if (talkedTo)
            {
                yield return StartCoroutine(UIHandler.instance.speak("Here's your hat. I risked my life for it.", "Me", plrMovement.instance.gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("FINALLY.", "Pirate", gameObject));
                GetComponent<SpriteRenderer>().sprite = hatted;
                yield return StartCoroutine(UIHandler.instance.speak("Now ``````THATS`````` better.", "Pirate", gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("Can we go now?", "Me", plrMovement.instance.gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("Climb aboard.", "Pirate", gameObject));
                StartCoroutine(UIHandler.instance.endingSequence());

            }
            else
            {
                yield return StartCoroutine(UIHandler.instance.speak("Hello sir.", "Me", plrMovement.instance.gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("It's```` CAPTAIN```` to you.", "Pirate", gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("Ok captain sir yes can you help me across this river?", "Me", plrMovement.instance.gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("I want you to take a good look at me", "Pirate", gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("and decide if I am fit for travel.", "Pirate", gameObject));
                yield return StartCoroutine(UIHandler.instance.speak(".```.```.```Yes?", "Me", plrMovement.instance.gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("NO.`````` NO I AM NOT!", "Pirate", gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("THOSE DEVILS TOOK MY HAT!", "Pirate", gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("And you can't leave because of that?", "Me", plrMovement.instance.gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("Are you kidding?````` I might as well puncture ten holes in this boat.", "Pirate", gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("You don't need to do that.``````` I have the hat right here.", "Me", plrMovement.instance.gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("FINALLY.", "Pirate", gameObject));
                GetComponent<SpriteRenderer>().sprite = hatted;
                yield return StartCoroutine(UIHandler.instance.speak("Now ``````THATS`````` better.", "Pirate", gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("Can we go now?", "Me", plrMovement.instance.gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("Climb aboard.", "Pirate", gameObject));
                StartCoroutine(UIHandler.instance.endingSequence());

            }
        }
        else
        {




            if (!talkedTo)
            {
                yield return StartCoroutine(UIHandler.instance.speak("Hello sir.", "Me", plrMovement.instance.gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("It's```` CAPTAIN```` to you.", "Pirate", gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("Ok captain sir yes can you help me across this river?", "Me", plrMovement.instance.gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("I want you to take a good look at me", "Pirate", gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("and decide if I am fit for travel.", "Pirate", gameObject));
                yield return StartCoroutine(UIHandler.instance.speak(".```.```.```Yes?", "Me", plrMovement.instance.gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("NO.`````` NO I AM NOT!", "Pirate", gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("THOSE DEVILS TOOK MY HAT!", "Pirate", gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("And you can't leave because of that?", "Me", plrMovement.instance.gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("Are you kidding?````` I might as well puncture ten holes in this boat.", "Pirate", gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("Look man, I'm having a really bad day.``` Can't you just let me through?", "Me", plrMovement.instance.gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("I'm sorry.``` I am not moving an INCH without my hat.", "Pirate", gameObject));
                yield return StartCoroutine(UIHandler.instance.speak("But if you would be so kind, the beast that took my hat went right.", "Pirate", gameObject));

                talkedTo = true;
            }
            else
            {
                yield return StartCoroutine(UIHandler.instance.speak("I'm so bald.....", "Pirate", gameObject));


            }
        }
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.pirateTalkedTo = talkedTo;
    }
    public void LoadData(GameData gameData)
    {
        talkedTo = gameData.pirateTalkedTo;
    }
}
