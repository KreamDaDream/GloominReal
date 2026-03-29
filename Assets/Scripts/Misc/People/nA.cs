using UnityEngine;
using System.Collections;
public class nA : MonoBehaviour, IInteractable
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

        yield return StartCoroutine(UIHandler.instance.speak("Did the Gloom steal your armour too?", "Guy", gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("No. I just don't like how hot it gets in the suit.", "Me", plrMovement.instance.gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("Then why do you still wear the helmet?", "Guy", gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("Aura.", "Me", plrMovement.instance.gameObject));





    }
}
