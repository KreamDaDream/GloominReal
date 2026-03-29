using System.Collections;
using UnityEngine;

public class electricanStuff : MonoBehaviour, IInteractable
{
    [SerializeField] private PipeSystem door;
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
        if (!door.isOpen)
        {
            yield return StartCoroutine(UIHandler.instance.speak("Aw man this sucks!", "???", gameObject));
            yield return StartCoroutine(UIHandler.instance.speak("If the door has no power,`````` how will I get to work?", "???", gameObject));
            yield return StartCoroutine(UIHandler.instance.speak("I have people to feed!", "???", gameObject));
            yield return StartCoroutine(UIHandler.instance.speak("That does sound pretty bad.", "Me", plrMovement.instance.gameObject));
            yield return StartCoroutine(UIHandler.instance.speak("I don't get paid enough as is as an electrician!", "Electrician", gameObject));
            yield return StartCoroutine(UIHandler.instance.speak("And now I can't even work!", "Electrician", gameObject));
            yield return StartCoroutine(UIHandler.instance.speak(".```.```.```Can't you just fix the door then?", "Me", plrMovement.instance.gameObject));
            yield return StartCoroutine(UIHandler.instance.speak("First of all,``````` I am not getting paid for that.", "Electrician", gameObject));
            yield return StartCoroutine(UIHandler.instance.speak("``````dude.", "Me", plrMovement.instance.gameObject));
            yield return StartCoroutine(UIHandler.instance.speak("Second of all,``````` There's like a`````` BUNCH`````` of Gloom there.", "Electrician", gameObject));
            yield return StartCoroutine(UIHandler.instance.speak("But if you want in,`````` you can go fix the pipes yourself.", "Electrician", gameObject));
            yield return StartCoroutine(UIHandler.instance.speak("Don't say I didn't warn ya though.", "Electrician", gameObject));
        } else
        {
            yield return StartCoroutine(UIHandler.instance.speak("Finally!```` I can go see my kids again!", "Electrician", gameObject));

        }

    }
}
