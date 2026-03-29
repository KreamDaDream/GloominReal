using UnityEngine;
using System.Collections;
public class oM : MonoBehaviour, IInteractable
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
        yield return StartCoroutine(UIHandler.instance.speak("What are you doing out here all alone?```` It's dangerous!", "Me", plrMovement.instance.gameObject));

        yield return StartCoroutine(UIHandler.instance.speak("Guffaw! These Gloomies are the real ones in danger!", "Old Man", gameObject));

        yield return StartCoroutine(UIHandler.instance.speak("I've let them know that if they dare touch me, I'll curse them!", "Old Man", gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("If only everybody back home was as lucky as you.", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("Luck?```` I don't need luck! I'm the Feared One. I'M INVINCIBLE!", "Old Man", gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("I'll keep you in my prayers.", "Me", plrMovement.instance.gameObject));




    }
}
