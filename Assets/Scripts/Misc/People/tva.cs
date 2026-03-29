using UnityEngine;
using System.Collections;
public class tva : MonoBehaviour, IInteractable
{


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

        yield return StartCoroutine(UIHandler.instance.speak("You look like you are on a mission.", "Knight", gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("Looks like we got a pscyhic here.", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("That's not how you spell it.", "Knight", gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("Huh?", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("Anyway, before you go further,", "Knight", gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("I've heard there's a sword that does twice as much damage around these parts.", "Knight", gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("Just saying.", "Knight", gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("It also may be on the left path.", "Knight", gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("Just saying.", "Knight", gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("It would really suck if all your battles were twice as long.", "Knight", gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("Just saying.", "Knight", gameObject));




    }
}
