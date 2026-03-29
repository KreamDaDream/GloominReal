using UnityEngine;
using System.Collections;
public class trappedGuy : MonoBehaviour, IInteractable
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

        yield return StartCoroutine(UIHandler.instance.speak("Oh thank Karoob! I've been trapped here for so long!", "Pipe Guy", gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("I came to fix the pipes,``` but the Gloom cornered me here.", "Pipe Guy", gameObject));

        yield return StartCoroutine(UIHandler.instance.speak("Well,``` you are free to go.", "Me", plrMovement.instance.gameObject));

        yield return StartCoroutine(UIHandler.instance.speak("Now that I think about it, I think I like standing under this tree.", "Pipe Guy", gameObject));
        yield return StartCoroutine(UIHandler.instance.speak("Ooooooookkkkkkkkkkkk.", "Me", plrMovement.instance.gameObject));




    }
}
